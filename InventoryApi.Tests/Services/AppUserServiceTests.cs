using InventoryApi.Models;
using InventoryApi.Models.Enums;
using InventoryApi.Repositories;
using InventoryApi.Services;
using Moq;
using Xunit;

namespace InventoryApi.Tests.Services;

public class AppUserServiceTests
{
    private readonly Mock<IAppUserRepository> _appUserRepositoryMock;
    private readonly AppUserService _appUserService;

    public AppUserServiceTests()
    {
        _appUserRepositoryMock = new Mock<IAppUserRepository>();
        _appUserService = new AppUserService(_appUserRepositoryMock.Object);
    }

    [Fact]
    public async Task ChangeUserRoleAsync_WhenUserDoesNotExist_ReturnsNull()
    {
        _appUserRepositoryMock
            .Setup(repo => repo.GetByIdAsync(999))
            .ReturnsAsync((AppUser?)null);

        var result = await _appUserService.ChangeUserRoleAsync(999, UserRole.Admin);

        Assert.Null(result);
    }

    [Fact]
    public async Task ChangeUserRoleAsync_WhenUserExists_UpdatesRoleAndPreservesOtherFields()
    {
        var existingUser = new AppUser(1, "Ana", "López", "ana@test.com", "70001111", 25, 60, true, "somehash", UserRole.User);

        _appUserRepositoryMock
            .Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(existingUser);

        _appUserRepositoryMock
            .Setup(repo => repo.UpdateAsync(1, It.IsAny<AppUser>()))
            .ReturnsAsync((int id, AppUser u) => u);

        var result = await _appUserService.ChangeUserRoleAsync(1, UserRole.Admin);

        Assert.NotNull(result);
        Assert.Equal(UserRole.Admin, result!.Role);
        Assert.Equal("Ana", result.Name);
    }
}