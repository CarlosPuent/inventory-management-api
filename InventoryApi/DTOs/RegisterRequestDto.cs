namespace InventoryApi.DTOs;

public record RegisterRequestDto(
    string Name,
    string LastName,
    string ContactEmail,
    string PhoneNumber,
    int Age,
    double Weight,
    string Password
);