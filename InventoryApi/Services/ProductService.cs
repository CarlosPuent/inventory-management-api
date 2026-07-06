using InventoryApi.Models;
using InventoryApi.Repositories;

namespace InventoryApi.Services
{
    public class ProductService: IProductService
    {

        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Task<List<Product>> GetAllProductsAsync()
        {
            return _productRepository.GetAllAsync();
        }

        public Task<Product?> GetProductByIdAsync(int id)
        {
            return _productRepository.GetByIdAsync(id);
        }

        public Task<Product> CreateProductAsync(Product product)
        {
            if (product.Price <= 0)
            {
                throw new ArgumentException("El precio del producto debe ser mayor a cero.");
            }

            return _productRepository.AddAsync(product);
        }
    }
}
