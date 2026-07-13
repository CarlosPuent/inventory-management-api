using InventoryApi.DTOs;
using InventoryApi.Models;

namespace InventoryApi.Services
{
    public interface ICategoryService
    {
        Task<PagedResult<Category>> GetPagedCategoriesAsync(int page, int pageSize);
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<Category> CreateCategoryAsync(Category category);
        Task<Category?> UpdateCategoryAsync(int id, Category category);
        Task<bool> DeleteCategoryAsync(int id);
    }
}