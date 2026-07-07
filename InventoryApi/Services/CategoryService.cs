using InventoryApi.Models;
using InventoryApi.Repositories;

namespace InventoryApi.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public Task<List<Category>> GetAllCategoriesAsync()
        {
            return _categoryRepository.GetAllAsync();
        }

        public Task<Category?> GetCategoryByIdAsync(int id)
        {
            return _categoryRepository.GetByIdAsync(id);
        }

        public Task<Category> CreateCategoryAsync(Category category)
        {
            ValidateCategoryBusinessRules(category);
            return _categoryRepository.AddAsync(category);
        }

        public async Task<Category?> UpdateCategoryAsync(int id, Category category)
        {
            ValidateCategoryBusinessRules(category);
            return await _categoryRepository.UpdateAsync(id, category);
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            return await _categoryRepository.DeleteAsync(id);
        }

        private void ValidateCategoryBusinessRules(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                throw new ArgumentException("El nombre de la categoría no puede estar vacío.");
            }

            if (string.IsNullOrWhiteSpace(category.Description) || category.Description.Length < 5 || category.Description.Length > 100)
            {
                throw new ArgumentException("La descripción de la categoría es obligatoria y debe tener entre 5 y 100 caracteres.");
            }
        }
    }
}