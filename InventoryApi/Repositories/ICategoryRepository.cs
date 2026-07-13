using InventoryApi.DTOs;
using InventoryApi.Models;

namespace InventoryApi.Repositories
{
    public interface ICategoryRepository
    {
        Task<PagedResult<Category>> GetFilteredAsync(CategoryFilterDto filter);
        Task<PagedResult<Category>> GetPagedAsync(int page, int pageSize);
        Task<List<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<Category> AddAsync(Category category);
        Task<Category?> UpdateAsync(int id, Category category);
        Task<bool> DeleteAsync(int id);
    }
}