namespace InventoryApi.DTOs;

public record AuthResponseDto(
    string Token,
    int UserId,
    string Name,
    string Role
);