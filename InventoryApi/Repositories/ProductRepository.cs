using InventoryApi.Data;
using InventoryApi.DTOs;
using InventoryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly InventoryDbContext _context;

    public ProductRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.Products.Include(p => p.Category).ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product> AddAsync(Product product)
    {
        var productToInsert = product with { Id = 0 };

        _context.Products.Add(productToInsert);
        await _context.SaveChangesAsync();

        return productToInsert;
    }

    public async Task<Product?> UpdateAsync(int id, Product product)
    {
        var existingProduct = await _context.Products.FindAsync(id);

        if (existingProduct == null)
        {
            return null;
        }

        _context.Entry(existingProduct).CurrentValues.SetValues(product with { Id = id });
        await _context.SaveChangesAsync();

        return existingProduct;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return false;
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<PagedResult<Product>> GetPagedAsync(int page, int pageSize)
    {
        var totalCount = await _context.Products.CountAsync();

        var items = await _context.Products
            .Include(p => p.Category)
            .OrderBy(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Product>(items, totalCount, page, pageSize);
    }

    public async Task<PagedResult<Product>> GetFilteredAsync(ProductFilterDto filter)
    {
        var query = _context.Products.Include(p => p.Category).AsQueryable();

        if (filter.CategoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == filter.CategoryId.Value);
        }

        if (filter.MinPrice.HasValue)
        {
            query = query.Where(p => p.Price >= filter.MinPrice.Value);
        }

        if (filter.MaxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= filter.MaxPrice.Value);
        }

        query = filter.SortBy?.ToLower() switch
        {
            "name" => filter.SortDirection == "desc" ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
            "price" => filter.SortDirection == "desc" ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
            "stock" => filter.SortDirection == "desc" ? query.OrderByDescending(p => p.Stock) : query.OrderBy(p => p.Stock),
            _ => filter.SortDirection == "desc" ? query.OrderByDescending(p => p.Id) : query.OrderBy(p => p.Id)
        };

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return new PagedResult<Product>(items, totalCount, filter.Page, filter.PageSize);
    }
}