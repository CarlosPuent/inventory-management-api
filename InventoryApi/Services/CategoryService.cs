using InventoryApi.DTOs;
using InventoryApi.Exceptions;
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

        public async Task<PagedResult<Category>> GetFilteredCategoriesAsync(CategoryFilterDto filter)
        {
            if (filter.Page < 1) throw new BusinessRuleException("El número de página debe ser mayor o igual a 1.");
            if (filter.PageSize < 1 || filter.PageSize > 100) throw new BusinessRuleException("El tamaño de página debe estar entre 1 y 100.");

            return await _categoryRepository.GetFilteredAsync(filter);
        }
        public async Task<PagedResult<Category>> GetPagedCategoriesAsync(int page, int pageSize)
        {
            if (page < 1)
            {
                throw new BusinessRuleException("El número de página debe ser mayor o igual a 1.");
            }

            if (pageSize < 1 || pageSize > 100)
            {
                throw new BusinessRuleException("El tamaño de página debe estar entre 1 y 100.");
            }

            return await _categoryRepository.GetPagedAsync(page, pageSize);
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
                throw new BusinessRuleException("El nombre de la categoría no puede estar vacío.");
            }

            if (string.IsNullOrWhiteSpace(category.Description) || category.Description.Length < 5 || category.Description.Length > 100)
            {
                throw new BusinessRuleException("La descripción de la categoría es obligatoria y debe tener entre 5 y 100 caracteres.");
            }
        }
    }
}