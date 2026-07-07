using InventoryApi.Models;
using InventoryApi.Repositories;

namespace InventoryApi.Services
{
    public class AppUsersService : IAppUsersService
    {
        private readonly IAppUserRepository _appUsersRepository;

        public AppUsersService(IAppUserRepository appUsersRepository)
        {
            _appUsersRepository = appUsersRepository;
        }

        public Task<List<AppUsers>> GetAllAppUsersAsync()
        {
            return _appUsersRepository.GetAllAsync();
        }

        public Task<AppUsers?> GetAppUserByIdAsync(int id)
        {
            return _appUsersRepository.GetByIdAsync(id);    
        }

        public Task<AppUsers> CreateAppUserAsync(AppUsers appUsers)
        {
            if (string.IsNullOrWhiteSpace(appUsers.Name)) 
            {
                throw new ArgumentException("El nombre del usuario no puede estar vació. ");
            }
            if (string.IsNullOrWhiteSpace(appUsers.LastName))
            {
                throw new ArgumentException("El apellido del usuario no puede estar vació. ");
            }
            if (string.IsNullOrWhiteSpace(appUsers.ContactEmail) || !appUsers.ContactEmail.Contains("@"))
            {
                throw new ArgumentException("Debe proporcionar un correo electrónico válido que contenga '@'.");
            }

            return _appUsersRepository.AddAsync(appUsers);

        }
    }
}
