using InventoryApi.Models;

namespace InventoryApi.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly List<Product> _products = new()
    {
        new Product(1, "Laptop Dell", 650.00m, 15),
        new Product(2, "Mouse Logitech", 12.50m, 3),
        new Product(3, "Teclado Mecánico", 45.00m, 0)
    };

    public Task<List<Product>> GetAllAsync()
    {
        return Task.FromResult(_products);
    }

    public Task<Product?> GetByIdAsync(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(product);
    }

    public Task<Product> AddAsync(Product product)
    {
        _products.Add(product);
        return Task.FromResult(product);
    }
}