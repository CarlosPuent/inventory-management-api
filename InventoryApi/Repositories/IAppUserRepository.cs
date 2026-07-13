using InventoryApi.Models;

namespace InventoryApi.Repositories
{
    public interface IAppUserRepository
    {
        Task<List<AppUser>> GetAllAsync();
        Task<AppUser?> GetByIdAsync(int id);
        Task<AppUser> AddAsync(AppUser appUser);
        Task<AppUser?> UpdateAsync(int id, AppUser appUser);
        Task<bool> DeleteAsync(int id);
        Task<AppUser?> GetByEmailAsync(string email);
    }
}