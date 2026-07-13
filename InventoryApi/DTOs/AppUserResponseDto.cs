using InventoryApi.Models.Enums;

namespace InventoryApi.DTOs;

public record AppUserResponseDto(
    int Id,
    string Name,
    string LastName,
    string ContactEmail,
    string PhoneNumber,
    int Age,
    double Weight,
    bool IsActive,
    UserRole Role
);