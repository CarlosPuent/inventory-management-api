namespace InventoryApi.DTOs;

public record SupplierResponseDto(
    int Id,
    string Name,
    string ContactEmail,
    bool IsActive
);