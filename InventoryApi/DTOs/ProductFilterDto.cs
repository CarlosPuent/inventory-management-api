namespace InventoryApi.DTOs;

public record ProductFilterDto(
    int Page = 1,
    int PageSize = 10,
    int? CategoryId = null,
    decimal? MinPrice = null,
    decimal? MaxPrice = null,
    string SortBy = "Id",
    string SortDirection = "asc"
);