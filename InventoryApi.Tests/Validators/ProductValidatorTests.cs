using InventoryApi.Models;
using InventoryApi.Validators;
using Xunit;

namespace InventoryApi.Tests.Validators;

public class ProductValidatorTests
{
    private readonly ProductValidator _validator;

    public ProductValidatorTests()
    {
        _validator = new ProductValidator();
    }

    [Fact]
    public void Validate_WhenPriceIsZeroOrNegative_HasValidationError()
    {
        var product = new Product(0, "Monitor", 0m, 10, 1);

        var result = _validator.Validate(product);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Price");
    }

    [Fact]
    public void Validate_WhenAllFieldsAreValid_IsValid()
    {
        var product = new Product(0, "Monitor", 150m, 10, 1);

        var result = _validator.Validate(product);

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_WhenNameIsEmptyOrWhitespace_HasValidationError(string invalidName)
    {
        var product = new Product(0, invalidName, 150m, 10, 1);

        var result = _validator.Validate(product);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name");
    }
}