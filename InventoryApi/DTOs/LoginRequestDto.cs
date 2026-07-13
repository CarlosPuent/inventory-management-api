namespace InventoryApi.DTOs;

public record LoginRequestDto(
    string ContactEmail,
    string Password
);