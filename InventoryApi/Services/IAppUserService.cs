using InventoryApi.Models;

namespace InventoryApi.Services
{
    public interface IAppUserService
    {
        Task<List<AppUser>> GetAllAppUsersAsync();
        Task<AppUser?> GetAppUserByIdAsync(int id);
        Task<AppUser> CreateAppUserAsync(AppUser appUser);
        Task<AppUser?> UpdateAppUserAsync(int id, AppUser appUser);
        Task<bool> DeleteAppUserAsync(int id);
    }
}