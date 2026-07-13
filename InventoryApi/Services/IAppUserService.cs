using InventoryApi.Models;

namespace InventoryApi.Services
{
    public interface IAppUserService
    {
        Task<List<AppUser>> GetAllAppUsersAsync();
        Task<AppUser?> GetAppUserByIdAsync(int id);
        Task<AppUser?> UpdateAppUserAsync(int id, AppUser appUser);
        Task<AppUser?> ChangeUserRoleAsync(int id, string newRole);
        Task<bool> DeleteAppUserAsync(int id);
    }
}