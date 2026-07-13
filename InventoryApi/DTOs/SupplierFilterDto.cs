namespace InventoryApi.DTOs;

public record SupplierFilterDto(
    int Page = 1,
    int PageSize = 10,
    bool? IsActive = null,
    string SortBy = "Id",
    string SortDirection = "asc"
);