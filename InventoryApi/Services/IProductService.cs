using InventoryApi.DTOs;
using InventoryApi.Models;

namespace InventoryApi.Services
{
    public interface IProductService
    {
        Task<PagedResult<Product>> GetPagedProductsAsync(int page, int pageSize);
        Task<List<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product> CreateProductAsync(Product product);
        Task<Product?> UpdateProductAsync(int id, Product product);
        Task<bool> DeleteProductAsync(int id);
    }
}