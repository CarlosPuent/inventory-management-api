using InventoryApi.Models;

namespace InventoryApi.Repositories
{
    public class AppUsersRepository : IAppUserRepository
    {
        private readonly List<AppUsers> _users = new()
        {
            new AppUsers(1, "Carlos", "Puente", "accpmurillo233@gmail.com", "74848800", 22, 202.2, true),
            new AppUsers(2, "Pedro", "Porro", "pedro@gmail.com", "71542589", 32, 182.2, true),
            new AppUsers(3, "Juan", "Camaney", "juan@gmail.com", "73984125", 28, 212.2, true),
            new AppUsers(4, "Julio", "Perez", "julio@gmail.com", "74147414", 32, 209.3, true)
        };

        public Task<List<AppUsers>> GetAllAsync()
        {
            return Task.FromResult(_users);
        }

        public Task<AppUsers?> GetByIdAsync(int id)
        {
            var appuser = _users.FirstOrDefault(u => u.Id == id);
            return Task.FromResult(appuser);
        }

        public Task<AppUsers> AddAsync(AppUsers appUsers)
        {

        var nextId = _users.Count == 0 ? 1 : _users.Max(usr => usr.Id) + 1;
        var created = appUsers with { Id = nextId };
        _users.Add(created);
        return Task.FromResult(created);

        }
    }
}
