namespace InventoryApi.DTOs;

public record ProductResponseDto(
    int Id,
    string Name,
    decimal Price,
    int Stock,
    int CategoryId,
    string? CategoryName
);