using InventoryApi.Data;
using InventoryApi.DTOs;
using InventoryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly InventoryDbContext _context;

        public CategoryRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Category>> GetPagedAsync(int page, int pageSize)
        {
            var totalCount = await _context.Categories.CountAsync();

            var items = await _context.Categories
                .OrderBy(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Category>(items, totalCount, page, pageSize);
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<Category> AddAsync(Category category)
        {
            var categoryToInsert = category with { Id = 0 };
            _context.Categories.Add(categoryToInsert);
            await _context.SaveChangesAsync();
            return categoryToInsert;
        }

        public async Task<Category?> UpdateAsync(int id, Category category)
        {
            var existingCategory = await _context.Categories.FindAsync(id);

            if (existingCategory == null)
            {
                return null;
            }

            _context.Entry(existingCategory).CurrentValues.SetValues(category with { Id = id });
            await _context.SaveChangesAsync();
            return existingCategory;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return false;
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}