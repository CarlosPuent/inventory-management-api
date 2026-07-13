using InventoryApi.Models;
using InventoryApi.Models.Enums;
using InventoryApi.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace InventoryApi.Tests.Services;

public class TokenServiceTests
{
    private TokenService CreateTokenService()
    {
        var inMemorySettings = new Dictionary<string, string?>
        {
            { "Jwt:Key", "ClaveDePruebaParaTestsUnitariosDeAlMenos32Caracteres" },
            { "Jwt:Issuer", "InventoryApiTests" },
            { "Jwt:Audience", "InventoryApiTestsUsers" },
            { "Jwt:ExpirationMinutes", "60" }
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        return new TokenService(configuration);
    }

    [Fact]
    public void GenerateToken_ReturnsNonEmptyString()
    {
        var tokenService = CreateTokenService();
        var user = new AppUser(1, "Ana", "López", "ana@test.com", "70001111", 25, 60, true, "hash", UserRole.Admin);

        var token = tokenService.GenerateToken(user);

        Assert.False(string.IsNullOrWhiteSpace(token));
    }

    [Fact]
    public void GenerateToken_ProducesTokenWithThreeParts()
    {
        var tokenService = CreateTokenService();
        var user = new AppUser(1, "Ana", "López", "ana@test.com", "70001111", 25, 60, true, "hash", UserRole.User);

        var token = tokenService.GenerateToken(user);
        var parts = token.Split('.');

        Assert.Equal(3, parts.Length);
    }
}