using InventoryApi.Models;
using InventoryApi.Validators;
using Xunit;

namespace InventoryApi.Tests.Validators;

public class SupplierValidatorTests
{
    private readonly SupplierValidator _validator = new();

    [Fact]
    public void Validate_WhenEmailFormatIsInvalid_HasValidationError()
    {
        var supplier = new Supplier(0, "Samsung", "no-es-un-email", true);

        var result = _validator.Validate(supplier);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "ContactEmail");
    }

    [Fact]
    public void Validate_WhenAllFieldsAreValid_IsValid()
    {
        var supplier = new Supplier(0, "Samsung", "contacto@samsung.com", true);

        var result = _validator.Validate(supplier);

        Assert.True(result.IsValid);
    }
}