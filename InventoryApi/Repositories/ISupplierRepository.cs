using InventoryApi.Models;

namespace InventoryApi.Repositories
{
    public interface ISupplierRepository
    {
        Task<List<Supplier>> GetAllAsync();
        Task<Supplier?> GetByIdAsync(int id);
        Task<Supplier> AddAsync(Supplier supplier);
        Task<Supplier?> UpdateAsync(int id, Supplier supplier);
        Task<bool> DeleteAsync(int id);
    }
}