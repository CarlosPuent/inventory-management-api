using InventoryApi.DTOs;
using InventoryApi.Models;
using InventoryApi.Models.Enums;

namespace InventoryApi.Services
{
    public interface IAppUserService
    {
        Task<PagedResult<AppUser>> GetFilteredAppUsersAsync(AppUserFilterDto filter);
        Task<PagedResult<AppUser>> GetPagedAppUsersAsync(int page, int pageSize);
        Task<List<AppUser>> GetAllAppUsersAsync();
        Task<AppUser?> GetAppUserByIdAsync(int id);
        Task<AppUser?> UpdateAppUserAsync(int id, AppUser appUser);
        Task<AppUser?> ChangeUserRoleAsync(int id, UserRole newRole);
        Task<bool> DeleteAppUserAsync(int id);
        Task<AppUser?> GetAppUserByEmailAsync(string email);
    }
}