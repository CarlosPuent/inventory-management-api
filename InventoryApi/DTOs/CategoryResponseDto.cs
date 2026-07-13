namespace InventoryApi.DTOs;

public record CategoryResponseDto(
    int Id,
    string Name,
    string Description,
    bool IsActive
);