using InventoryApi.Models;
using InventoryApi.Repositories;
using InventoryApi.Services;
using Moq;
using Xunit;

namespace InventoryApi.Tests.Services;

public class SupplierServiceTests
{
    private readonly Mock<ISupplierRepository> _supplierRepositoryMock;
    private readonly SupplierService _supplierService;

    public SupplierServiceTests()
    {
        _supplierRepositoryMock = new Mock<ISupplierRepository>();
        _supplierService = new SupplierService(_supplierRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateSupplierAsync_CallsRepositoryAddAsync()
    {
        var supplier = new Supplier(0, "Samsung", "contacto@samsung.com", true);

        _supplierRepositoryMock
            .Setup(repo => repo.AddAsync(supplier))
            .ReturnsAsync(supplier with { Id = 3 });

        var result = await _supplierService.CreateSupplierAsync(supplier);

        Assert.Equal(3, result.Id);
        _supplierRepositoryMock.Verify(repo => repo.AddAsync(supplier), Times.Once);
    }

    [Fact]
    public async Task UpdateSupplierAsync_WhenSupplierDoesNotExist_ReturnsNull()
    {
        var supplier = new Supplier(0, "Samsung", "contacto@samsung.com", true);

        _supplierRepositoryMock
            .Setup(repo => repo.UpdateAsync(999, supplier))
            .ReturnsAsync((Supplier?)null);

        var result = await _supplierService.UpdateSupplierAsync(999, supplier);

        Assert.Null(result);
    }
}