using InventoryApi.Models;

namespace InventoryApi.Repositories
{
    public interface IAppUserRepository
    {
        Task<List<AppUsers>> GetAllAsync();
        Task<AppUsers?> GetByIdAsync(int id);
        Task<AppUsers> AddAsync(AppUsers appUsers);
    }
}
