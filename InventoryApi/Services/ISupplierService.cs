using InventoryApi.DTOs;
using InventoryApi.Models;

namespace InventoryApi.Services
{
    public interface ISupplierService
    {
        Task<PagedResult<Supplier>> GetPagedSuppliersAsync(int page, int pageSize);
        Task<List<Supplier>> GetAllSuppliersAsync();
        Task<Supplier?> GetSupplierByIdAsync(int id);
        Task<Supplier> CreateSupplierAsync(Supplier supplier);
        Task<Supplier?> UpdateSupplierAsync(int id, Supplier supplier);
        Task<bool> DeleteSupplierAsync(int id);
    }
}