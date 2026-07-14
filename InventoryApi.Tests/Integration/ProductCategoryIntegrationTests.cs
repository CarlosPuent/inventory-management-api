using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using InventoryApi.DTOs;
using InventoryApi.Data;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace InventoryApi.Tests.Integration;

public class ProductCategoryIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ProductCategoryIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();

        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
        db.Database.EnsureCreated();
    }

    private async Task AuthenticateAsync(string emailSuffix)
    {
        var email = $"integration.{emailSuffix}@test.com";

        var registerRequest = new RegisterRequestDto(
            "Test", "User", email, "70009999", 30, 70, "ClavePrueba123"
        );
        await _client.PostAsJsonAsync("/api/Auth/register", registerRequest);

        var loginRequest = new LoginRequestDto(email, "ClavePrueba123");
        var loginResponse = await _client.PostAsJsonAsync("/api/Auth/login", loginRequest);
        var authResult = await loginResponse.Content.ReadFromJsonAsync<AuthResponseDto>();

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", authResult!.Token);
    }

    [Fact]
    public async Task CreateProduct_WithValidCategory_Returns201AndPersistsWithRelation()
    {
        await AuthenticateAsync("create-valid");

        var categoryRequest = new
        {
            Id = 0,
            Name = "Electronica Integration",
            Description = "Categoria de prueba de integracion",
            IsActive = true
        };
        var categoryResponse = await _client.PostAsJsonAsync("/api/Category", categoryRequest);
        var createdCategory = await categoryResponse.Content.ReadFromJsonAsync<CategoryResponseDto>();

        var productRequest = new
        {
            Id = 0,
            Name = "Monitor Integration",
            Price = 199.99m,
            Stock = 5,
            CategoryId = createdCategory!.Id
        };
        var productResponse = await _client.PostAsJsonAsync("/api/Products", productRequest);

        Assert.Equal(HttpStatusCode.Created, productResponse.StatusCode);

        var createdProduct = await productResponse.Content.ReadFromJsonAsync<ProductResponseDto>();
        Assert.NotNull(createdProduct);
        Assert.Equal("Electronica Integration", createdProduct!.CategoryName);
    }

    [Fact]
    public async Task CreateProduct_WithNonExistentCategory_Returns400()
    {
        await AuthenticateAsync("create-invalid");

        var productRequest = new
        {
            Id = 0,
            Name = "Producto Huerfano",
            Price = 50m,
            Stock = 3,
            CategoryId = 99999
        };
        var response = await _client.PostAsJsonAsync("/api/Products", productRequest);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task DeleteCategory_WithAssociatedProducts_Returns409()
    {
        await AuthenticateAsync("delete-conflict");

        var categoryRequest = new
        {
            Id = 0,
            Name = "Categoria Con Productos",
            Description = "No debería poder borrarse",
            IsActive = true
        };
        var categoryResponse = await _client.PostAsJsonAsync("/api/Category", categoryRequest);
        var createdCategory = await categoryResponse.Content.ReadFromJsonAsync<CategoryResponseDto>();

        var productRequest = new
        {
            Id = 0,
            Name = "Producto Asociado",
            Price = 25m,
            Stock = 10,
            CategoryId = createdCategory!.Id
        };
        await _client.PostAsJsonAsync("/api/Products", productRequest);

        var deleteResponse = await _client.DeleteAsync($"/api/Category/{createdCategory.Id}");

        Assert.Equal(HttpStatusCode.Conflict, deleteResponse.StatusCode);
    }

    [Fact]
    public async Task GetPagedProducts_ReturnsCorrectPageSize()
    {
        await AuthenticateAsync("pagination");

        var categoryRequest = new
        {
            Id = 0,
            Name = "Categoria Paginacion",
            Description = "Para probar paginacion real",
            IsActive = true
        };
        var categoryResponse = await _client.PostAsJsonAsync("/api/Category", categoryRequest);
        var createdCategory = await categoryResponse.Content.ReadFromJsonAsync<CategoryResponseDto>();

        for (int i = 1; i <= 3; i++)
        {
            var productRequest = new
            {
                Id = 0,
                Name = $"Producto Paginado {i}",
                Price = 10m * i,
                Stock = i,
                CategoryId = createdCategory!.Id
            };
            await _client.PostAsJsonAsync("/api/Products", productRequest);
        }

        var response = await _client.GetAsync("/api/Products/paged?page=1&pageSize=2");
        var result = await response.Content.ReadFromJsonAsync<PagedResult<ProductResponseDto>>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(result);
        Assert.Equal(2, result!.Items.Count);
    }
}