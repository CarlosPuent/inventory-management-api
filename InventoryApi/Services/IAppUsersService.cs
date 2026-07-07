using InventoryApi.Models;
using InventoryApi.Repositories;

namespace InventoryApi.Services
{
    public interface IAppUsersService
    {
        Task<List<AppUsers>> GetAllAppUsersAsync();
        Task<AppUsers?> GetAppUserByIdAsync(int id);
        Task<AppUsers> CreateAppUserAsync(AppUsers appUsers);
    }
}
