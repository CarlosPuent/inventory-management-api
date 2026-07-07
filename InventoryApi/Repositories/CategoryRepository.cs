using InventoryApi.Data;
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
    }
}