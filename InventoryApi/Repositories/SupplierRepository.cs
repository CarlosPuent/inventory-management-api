using InventoryApi.Data;
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