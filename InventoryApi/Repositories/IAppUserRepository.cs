using InventoryApi.Models;

namespace InventoryApi.Repositories
{
    public interface IAppUserRepository
    {
        Task<List<AppUser>> GetAllAsync();
        Task<AppUser?> GetByIdAsync(int id);
        Task<AppUser> AddAsync(AppUser appUser);
    }
}