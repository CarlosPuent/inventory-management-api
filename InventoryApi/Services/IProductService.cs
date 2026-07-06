using InventoryApi.Models;

namespace InventoryApi.Services
{
    public interface IProductService
    {

        Task<List<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product> CreateProductAsync(Product product);


    }
}
