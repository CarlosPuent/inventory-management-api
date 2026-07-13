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

        public async Task<AppUser?> UpdateAsync(int id, AppUser appUser)
        {
            var existingAppUser = await _context.AppUsers.FindAsync(id);

            if (existingAppUser == null)
            {
                return null;
            }

            _context.Entry(existingAppUser).CurrentValues.SetValues(appUser with { Id = id });
            await _context.SaveChangesAsync();

            return existingAppUser;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var appUser = await _context.AppUsers.FindAsync(id);

            if (appUser == null)
            {
                return false;
            }

            _context.AppUsers.Remove(appUser);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<AppUser?> GetByEmailAsync(string email)
        {
            return await _context.AppUsers.FirstOrDefaultAsync(u => u.ContactEmail == email);
        }
    }
}