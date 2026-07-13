using InventoryApi.Exceptions;
using InventoryApi.Models;
using InventoryApi.Repositories;
using InventoryApi.Services;
using Moq;
using Xunit;

namespace InventoryApi.Tests.Services;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _productService = new ProductService(_productRepositoryMock.Object, _categoryRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateProductAsync_WhenCategoryDoesNotExist_ThrowsBusinessRuleException()
    {
        // Arrange
        var product = new Product(0, "Monitor", 150m, 10, CategoryId: 999);

        _categoryRepositoryMock
            .Setup(repo => repo.GetByIdAsync(999))
            .ReturnsAsync((Category?)null);

        // Act & Assert
        await Assert.ThrowsAsync<BusinessRuleException>(
            () => _productService.CreateProductAsync(product)
        );
    }

    [Fact]
    public async Task CreateProductAsync_WhenCategoryExists_CallsRepositoryAddAsync()
    {
        // Arrange
        var category = new Category(1, "Electrónicos", "Dispositivos varios", true);
        var product = new Product(0, "Monitor", 150m, 10, CategoryId: 1);

        _categoryRepositoryMock
            .Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(category);

        _productRepositoryMock
            .Setup(repo => repo.AddAsync(product))
            .ReturnsAsync(product with { Id = 5 });

        // Act
        var result = await _productService.CreateProductAsync(product);

        // Assert
        Assert.Equal(5, result.Id);
        _productRepositoryMock.Verify(repo => repo.AddAsync(product), Times.Once);
    }
}