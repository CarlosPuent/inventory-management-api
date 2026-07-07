using InventoryApi.Data;
using InventoryApi.Models;
using Microsoft.EntityFrameworkCore; 

namespace InventoryApi.Repositories
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly InventoryDbContext _context;

        public AppUserRepository(InventoryDbContext context)
        {
            _context = context;
        }


        public async Task<List<AppUser>> GetAllAsync()
        {
            return await _context.AppUsers.ToListAsync();
        }

        public async Task<AppUser?> GetByIdAsync(int id)
        {
            return await _context.AppUsers.FindAsync(id);
        }

        public async Task<AppUser> AddAsync(AppUser appUser)
        {
            var appUserToInsert = appUser with { Id = 0 };


            _context.AppUsers.Add(appUserToInsert);
            await _context.SaveChangesAsync();

            return appUserToInsert;
        }
    }
}