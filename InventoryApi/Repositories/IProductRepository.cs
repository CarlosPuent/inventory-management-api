using InventoryApi.DTOs;
using InventoryApi.Models;

namespace InventoryApi.Repositories
{
    public interface IProductRepository
    {
        Task<PagedResult<Product>> GetFilteredAsync(ProductFilterDto filter);
        Task<PagedResult<Product>> GetPagedAsync(int page, int pageSize);
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product> AddAsync(Product product);

        Task<Product?> UpdateAsync(int id, Product product);
        Task<bool> DeleteAsync(int id);
    }
}