using InventoryApi.Models;

namespace InventoryApi.Repositories
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly List<AppUser> _users = new()
        {
            new AppUser(1, "Carlos", "Puente", "accpmurillo233@gmail.com", "74848800", 22, 202.2, true),
            new AppUser(2, "Pedro", "Porro", "pedro@gmail.com", "71542589", 32, 182.2, true),
            new AppUser(3, "Juan", "Camaney", "juan@gmail.com", "73984125", 28, 212.2, true),
            new AppUser(4, "Julio", "Perez", "julio@gmail.com", "74147414", 32, 209.3, true)
        };

        public Task<List<AppUser>> GetAllAsync()
        {
            return Task.FromResult(_users);
        }

        public Task<AppUser?> GetByIdAsync(int id)
        {
            var appUser = _users.FirstOrDefault(u => u.Id == id);
            return Task.FromResult(appUser);
        }

        public Task<AppUser> AddAsync(AppUser appUser)
        {
            var nextId = _users.Count == 0 ? 1 : _users.Max(u => u.Id) + 1;
            var created = appUser with { Id = nextId };
            _users.Add(created);
            return Task.FromResult(created);
        }
    }
}