using InventoryApi.Models;
using InventoryApi.Repositories;

namespace InventoryApi.Services
{
    public class AppUserService : IAppUserService
    {
        private readonly IAppUserRepository _appUserRepository;

        public AppUserService(IAppUserRepository appUserRepository)
        {
            _appUserRepository = appUserRepository;
        }

        public Task<List<AppUser>> GetAllAppUsersAsync()
        {
            return _appUserRepository.GetAllAsync();
        }

        public Task<AppUser?> GetAppUserByIdAsync(int id)
        {
            return _appUserRepository.GetByIdAsync(id);
        }

        public Task<AppUser> CreateAppUserAsync(AppUser appUser)
        {
            ValidateAppUserBusinessRules(appUser);
            return _appUserRepository.AddAsync(appUser);
        }

        public async Task<AppUser?> UpdateAppUserAsync(int id, AppUser appUser)
        {
            ValidateAppUserBusinessRules(appUser);
            return await _appUserRepository.UpdateAsync(id, appUser);
        }

        public async Task<bool> DeleteAppUserAsync(int id)
        {
            return await _appUserRepository.DeleteAsync(id);
        }

        private void ValidateAppUserBusinessRules(AppUser appUser)
        {
            if (string.IsNullOrWhiteSpace(appUser.Name))
            {
                throw new ArgumentException("El nombre del usuario no puede estar vacío.");
            }

            if (string.IsNullOrWhiteSpace(appUser.LastName))
            {
                throw new ArgumentException("El apellido del usuario no puede estar vacío.");
            }

            if (string.IsNullOrWhiteSpace(appUser.ContactEmail) || !appUser.ContactEmail.Contains("@"))
            {
                throw new ArgumentException("Debe proporcionar un correo electrónico válido que contenga '@'.");
            }

            if (string.IsNullOrWhiteSpace(appUser.PhoneNumber) || appUser.PhoneNumber.Length < 8)
            {
                throw new ArgumentException("Debe proporcionar un número de teléfono válido de al menos 8 dígitos.");
            }

            if (appUser.Age <= 0 || appUser.Age > 120)
            {
                throw new ArgumentException("La edad debe estar entre 1 y 120 años.");
            }

            if (appUser.Weight <= 0)
            {
                throw new ArgumentException("El peso debe ser mayor a cero.");
            }
        }
    }
}