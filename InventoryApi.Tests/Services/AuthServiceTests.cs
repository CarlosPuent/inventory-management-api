using InventoryApi.DTOs;
using InventoryApi.Exceptions;
using InventoryApi.Models;
using InventoryApi.Models.Enums;
using InventoryApi.Repositories;
using InventoryApi.Services;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace InventoryApi.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IAppUserRepository> _appUserRepositoryMock;
    private readonly Mock<IPasswordHasher<AppUser>> _passwordHasherMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _appUserRepositoryMock = new Mock<IAppUserRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher<AppUser>>();
        _tokenServiceMock = new Mock<ITokenService>();

        _authService = new AuthService(
            _appUserRepositoryMock.Object,
            _passwordHasherMock.Object,
            _tokenServiceMock.Object
        );
    }

    [Fact]
    public async Task RegisterAsync_WhenEmailAlreadyExists_ThrowsConflictException()
    {
        var existingUser = new AppUser(1, "Ana", "López", "ana@test.com", "70001111", 25, 60, true, "hashedPassword", UserRole.User);

        _appUserRepositoryMock
            .Setup(repo => repo.GetByEmailAsync("ana@test.com"))
            .ReturnsAsync(existingUser);

        var request = new RegisterRequestDto("Ana", "López", "ana@test.com", "70001111", 25, 60, "ClaveNueva123");

        await Assert.ThrowsAsync<ConflictException>(
            () => _authService.RegisterAsync(request)
        );
    }

    [Fact]
    public async Task LoginAsync_WhenUserDoesNotExist_ReturnsNull()
    {
        _appUserRepositoryMock
            .Setup(repo => repo.GetByEmailAsync("noexiste@test.com"))
            .ReturnsAsync((AppUser?)null);

        var request = new LoginRequestDto("noexiste@test.com", "cualquierClave");

        var result = await _authService.LoginAsync(request);

        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_WhenPasswordIsIncorrect_ReturnsNull()
    {
        var existingUser = new AppUser(1, "Ana", "López", "ana@test.com", "70001111", 25, 60, true, "hashedPassword", UserRole.User);

        _appUserRepositoryMock
            .Setup(repo => repo.GetByEmailAsync("ana@test.com"))
            .ReturnsAsync(existingUser);

        _passwordHasherMock
            .Setup(hasher => hasher.VerifyHashedPassword(existingUser, "hashedPassword", "claveIncorrecta"))
            .Returns(PasswordVerificationResult.Failed);

        var request = new LoginRequestDto("ana@test.com", "claveIncorrecta");

        var result = await _authService.LoginAsync(request);

        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_WhenCredentialsAreCorrect_ReturnsAuthResponseWithToken()
    {
        var existingUser = new AppUser(1, "Ana", "López", "ana@test.com", "70001111", 25, 60, true, "hashedPassword", UserRole.Admin);

        _appUserRepositoryMock
            .Setup(repo => repo.GetByEmailAsync("ana@test.com"))
            .ReturnsAsync(existingUser);

        _passwordHasherMock
            .Setup(hasher => hasher.VerifyHashedPassword(existingUser, "hashedPassword", "claveCorrecta"))
            .Returns(PasswordVerificationResult.Success);

        _tokenServiceMock
            .Setup(ts => ts.GenerateToken(existingUser))
            .Returns("un-token-jwt-simulado");

        var request = new LoginRequestDto("ana@test.com", "claveCorrecta");

        var result = await _authService.LoginAsync(request);

        Assert.NotNull(result);
        Assert.Equal("un-token-jwt-simulado", result!.Token);
        Assert.Equal("Admin", result.Role);
    }
}