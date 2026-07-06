using InventoryApi.Models;

namespace InventoryApi.Services
{
    public interface ISupplierService
    {

        Task<List<Supplier>> GetAllSuppliersAsync();
        Task<Supplier?> GetSupplierByIdAsync(int id);
        Task<Supplier> CreateSupplierAsync(Supplier supplier);
    }
}
