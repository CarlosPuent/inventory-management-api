using InventoryApi.Data;
using InventoryApi.DTOs;
using InventoryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly InventoryDbContext _context;

        public SupplierRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Supplier>> GetFilteredAsync(SupplierFilterDto filter)
        {
            var query = _context.Suppliers.AsQueryable();

            if (filter.IsActive.HasValue)
            {
                query = query.Where(s => s.IsActive == filter.IsActive.Value);
            }

            query = filter.SortBy?.ToLower() switch
            {
                "name" => filter.SortDirection == "desc" ? query.OrderByDescending(s => s.Name) : query.OrderBy(s => s.Name),
                _ => filter.SortDirection == "desc" ? query.OrderByDescending(s => s.Id) : query.OrderBy(s => s.Id)
            };

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return new PagedResult<Supplier>(items, totalCount, filter.Page, filter.PageSize);
        }

        public async Task<PagedResult<Supplier>> GetPagedAsync(int page, int pageSize)
        {
            var totalCount = await _context.Suppliers.CountAsync();

            var items = await _context.Suppliers
                .OrderBy(s => s.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Supplier>(items, totalCount, page, pageSize);
        }

        public async Task<List<Supplier>> GetAllAsync()
        {
            return await _context.Suppliers.ToListAsync();
        }

        public async Task<Supplier?> GetByIdAsync(int id)
        {
            return await _context.Suppliers.FindAsync(id);
        }

        public async Task<Supplier> AddAsync(Supplier supplier)
        {
            var supplierToInsert = supplier with { Id = 0 };
            _context.Suppliers.Add(supplierToInsert);
            await _context.SaveChangesAsync();
            return supplierToInsert;
        }

        public async Task<Supplier?> UpdateAsync(int id, Supplier supplier)
        {
            var existingSupplier = await _context.Suppliers.FindAsync(id);

            if (existingSupplier == null)
            {
                return null;
            }

            _context.Entry(existingSupplier).CurrentValues.SetValues(supplier with { Id = id });
            await _context.SaveChangesAsync();
            return existingSupplier;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
            {
                return false;
            }

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}