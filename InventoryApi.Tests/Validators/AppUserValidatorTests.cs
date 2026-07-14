using InventoryApi.Models;
using InventoryApi.Models.Enums;
using InventoryApi.Validators;
using Xunit;

namespace InventoryApi.Tests.Validators;

public class AppUserValidatorTests
{
    private readonly AppUserValidator _validator = new();

    [Fact]
    public void Validate_WhenAgeIsOutOfRange_HasValidationError()
    {
        var appUser = new AppUser(0, "Ana", "Lopez", "ana@test.com", "70001111", 200, 60, true, "", UserRole.User);

        var result = _validator.Validate(appUser);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Age");
    }

    [Fact]
    public void Validate_WhenPhoneNumberIsTooShort_HasValidationError()
    {
        var appUser = new AppUser(0, "Ana", "Lopez", "ana@test.com", "123", 25, 60, true, "", UserRole.User);

        var result = _validator.Validate(appUser);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "PhoneNumber");
    }

    [Fact]
    public void Validate_WhenAllFieldsAreValid_IsValid()
    {
        var appUser = new AppUser(0, "Ana", "Lopez", "ana@test.com", "70001111", 25, 60, true, "", UserRole.User);

        var result = _validator.Validate(appUser);

        Assert.True(result.IsValid);
    }
}