using InventoryApi.Models;
using InventoryApi.Models.Enums;

namespace InventoryApi.Services
{
    public interface IAppUserService
    {
        Task<List<AppUser>> GetAllAppUsersAsync();
        Task<AppUser?> GetAppUserByIdAsync(int id);
        Task<AppUser?> UpdateAppUserAsync(int id, AppUser appUser);
        Task<AppUser?> ChangeUserRoleAsync(int id, UserRole newRole);
        Task<bool> DeleteAppUserAsync(int id);
    }
}