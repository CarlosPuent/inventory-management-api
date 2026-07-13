using InventoryApi.Models;
using InventoryApi.Repositories;
using InventoryApi.Services;
using Moq;
using Xunit;

namespace InventoryApi.Tests.Services;

public class CategoryServiceTests
{
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly CategoryService _categoryService;

    public CategoryServiceTests()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _categoryService = new CategoryService(_categoryRepositoryMock.Object);
    }

    [Fact]
    public async Task DeleteCategoryAsync_WhenCategoryDoesNotExist_ReturnsFalse()
    {
        _categoryRepositoryMock
            .Setup(repo => repo.DeleteAsync(999))
            .ReturnsAsync(false);

        var result = await _categoryService.DeleteCategoryAsync(999);

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteCategoryAsync_WhenCategoryExists_ReturnsTrue()
    {
        _categoryRepositoryMock
            .Setup(repo => repo.DeleteAsync(1))
            .ReturnsAsync(true);

        var result = await _categoryService.DeleteCategoryAsync(1);

        Assert.True(result);
        _categoryRepositoryMock.Verify(repo => repo.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetCategoryByIdAsync_WhenCategoryExists_ReturnsCategory()
    {
        var category = new Category(1, "Electrónicos", "Dispositivos varios", true);

        _categoryRepositoryMock
            .Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(category);

        var result = await _categoryService.GetCategoryByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Electrónicos", result!.Name);
    }
}