using InventoryApi.Data;
using InventoryApi.DTOs;
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

        public async Task<PagedResult<AppUser>> GetFilteredAsync(AppUserFilterDto filter)
        {
            var query = _context.AppUsers.AsQueryable();

            if (filter.Role.HasValue)
            {
                query = query.Where(u => u.Role == filter.Role.Value);
            }

            if (filter.IsActive.HasValue)
            {
                query = query.Where(u => u.IsActive == filter.IsActive.Value);
            }

            query = filter.SortBy?.ToLower() switch
            {
                "name" => filter.SortDirection == "desc" ? query.OrderByDescending(u => u.Name) : query.OrderBy(u => u.Name),
                "age" => filter.SortDirection == "desc" ? query.OrderByDescending(u => u.Age) : query.OrderBy(u => u.Age),
                _ => filter.SortDirection == "desc" ? query.OrderByDescending(u => u.Id) : query.OrderBy(u => u.Id)
            };

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return new PagedResult<AppUser>(items, totalCount, filter.Page, filter.PageSize);
        }

        public async Task<PagedResult<AppUser>> GetPagedAsync(int page, int pageSize)
        {
            var totalCount = await _context.AppUsers.CountAsync();

            var items = await _context.AppUsers
                .OrderBy(u => u.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<AppUser>(items, totalCount, page, pageSize);
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

        // Tu nuevo método intacto
        public async Task<AppUser?> GetByEmailAsync(string email)
        {
            return await _context.AppUsers.FirstOrDefaultAsync(u => u.ContactEmail == email);
        }
    }
}