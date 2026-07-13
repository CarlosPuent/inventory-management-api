namespace InventoryApi.DTOs;

public record PaginationRequestDto(int Page = 1, int PageSize = 10);