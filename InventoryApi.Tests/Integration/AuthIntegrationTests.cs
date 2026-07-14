using InventoryApi.Data;
using InventoryApi.DTOs;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Xunit;

namespace InventoryApi.Tests.Integration;

public class AuthIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();

        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
        db.Database.EnsureCreated();
    }

    [Fact]
    public async Task Register_WithValidData_Returns200AndToken()
    {
        var request = new RegisterRequestDto(
            "Carlos", "Puente", "carlos.integration@test.com", "70001111", 25, 70, "ClaveSegura123"
        );

        var response = await _client.PostAsJsonAsync("/api/Auth/register", request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        Assert.NotNull(body);
        Assert.False(string.IsNullOrWhiteSpace(body!.Token));
    }

    [Fact]
    public async Task Register_WithDuplicateEmail_Returns409()
    {
        var request = new RegisterRequestDto(
            "Pedro", "Gomez", "pedro.duplicado@test.com", "70003333", 28, 75, "ClaveOriginal123"
        );

        await _client.PostAsJsonAsync("/api/Auth/register", request);
        var secondResponse = await _client.PostAsJsonAsync("/api/Auth/register", request);

        Assert.Equal(HttpStatusCode.Conflict, secondResponse.StatusCode);
    }

    [Fact]
    public async Task ProtectedEndpoint_WithoutToken_Returns401()
    {
        var response = await _client.GetAsync("/api/Products");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task RegisterThenLogin_WithCorrectCredentials_AllowsAccessToProtectedEndpoint()
    {
        var registerRequest = new RegisterRequestDto(
            "Maria", "Lopez", "maria.integration@test.com", "70002222", 30, 65, "OtraClave456"
        );
        await _client.PostAsJsonAsync("/api/Auth/register", registerRequest);

        var loginRequest = new LoginRequestDto("maria.integration@test.com", "OtraClave456");
        var loginResponse = await _client.PostAsJsonAsync("/api/Auth/login", loginRequest);
        var authResult = await loginResponse.Content.ReadFromJsonAsync<AuthResponseDto>();

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", authResult!.Token);

        var protectedResponse = await _client.GetAsync("/api/Products");

        Assert.Equal(HttpStatusCode.OK, protectedResponse.StatusCode);
    }
}