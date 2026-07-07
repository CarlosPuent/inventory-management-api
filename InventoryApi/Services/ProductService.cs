using InventoryApi.Models;
using InventoryApi.Repositories;

namespace InventoryApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public Task<List<Product>> GetAllProductsAsync()
        {
            return _productRepository.GetAllAsync();
        }

        public Task<Product?> GetProductByIdAsync(int id)
        {
            return _productRepository.GetByIdAsync(id);
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            if (product.Price <= 0)
            {
                throw new ArgumentException("El precio del producto debe ser mayor a cero.");
            }

            var categoryExists = await _categoryRepository.GetByIdAsync(product.CategoryId);

            if (categoryExists == null)
            {
                throw new ArgumentException("La categoría especificada no existe.");
            }

            return await _productRepository.AddAsync(product);
        }
    }
}