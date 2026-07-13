using InventoryApi.Models.Enums;

namespace InventoryApi.DTOs;

public record AppUserFilterDto(
    int Page = 1,
    int PageSize = 10,
    UserRole? Role = null,
    bool? IsActive = null,
    string SortBy = "Id",
    string SortDirection = "asc"
);