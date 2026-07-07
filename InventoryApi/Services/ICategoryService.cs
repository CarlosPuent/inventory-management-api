using InventoryApi.Models;

namespace InventoryApi.Services
{
    public interface ICategoryService
    {

        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<Category> CreateCategoryAsync(Category category);
    }
}
