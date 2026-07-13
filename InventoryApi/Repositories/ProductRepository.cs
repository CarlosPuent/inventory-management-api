using InventoryApi.Data;
using InventoryApi.DTOs;
using InventoryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly InventoryDbContext _context;

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
}