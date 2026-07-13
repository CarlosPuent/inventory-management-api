using InventoryApi.Models;
using InventoryApi.Validators;
using Xunit;

namespace InventoryApi.Tests.Validators;

public class CategoryValidatorTests
{
    private readonly CategoryValidator _validator = new();

    [Fact]
    public void Validate_WhenNameIsEmpty_HasValidationError()
    {
        var category = new Category(0, "", "Descripcion valida", true);

        var result = _validator.Validate(category);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Name");
    }

    [Fact]
    public void Validate_WhenDescriptionIsTooShort_HasValidationError()
    {
        var category = new Category(0, "Nombre valido", "abc", true);

        var result = _validator.Validate(category);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Description");
    }

    [Fact]
    public void Validate_WhenAllFieldsAreValid_IsValid()
    {
        var category = new Category(0, "Electronicos", "Descripcion suficientemente larga", true);

        var result = _validator.Validate(category);

        Assert.True(result.IsValid);
    }
}