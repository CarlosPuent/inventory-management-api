using InventoryApi.DTOs;
using InventoryApi.Exceptions;
using InventoryApi.Models;
using InventoryApi.Models.Enums;
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

        public async Task<PagedResult<AppUser>> GetFilteredAppUsersAsync(AppUserFilterDto filter)
        {
            if (filter.Page < 1)
            {
                throw new BusinessRuleException("El número de página debe ser mayor o igual a 1.");
            }

            if (filter.PageSize < 1 || filter.PageSize > 100)
            {
                throw new BusinessRuleException("El tamaño de página debe estar entre 1 y 100.");
            }

            return await _appUserRepository.GetFilteredAsync(filter);
        }

        public async Task<PagedResult<AppUser>> GetPagedAppUsersAsync(int page, int pageSize)
        {
            if (page < 1)
            {
                throw new BusinessRuleException("El número de página debe ser mayor o igual a 1.");
            }

            if (pageSize < 1 || pageSize > 100)
            {
                throw new BusinessRuleException("El tamaño de página debe estar entre 1 y 100.");
            }

            return await _appUserRepository.GetPagedAsync(page, pageSize);
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
            var existingAppUser = await _appUserRepository.GetByIdAsync(id);

            if (existingAppUser == null)
            {
                return null;
            }

            ValidateAppUserBusinessRules(appUser);

            var appUserToUpdate = appUser with
            {
                Role = existingAppUser.Role,
                PasswordHash = existingAppUser.PasswordHash
            };

            return await _appUserRepository.UpdateAsync(id, appUserToUpdate);
        }

        public async Task<AppUser?> ChangeUserRoleAsync(int id, UserRole newRole)
        {
            var existingAppUser = await _appUserRepository.GetByIdAsync(id);

            if (existingAppUser == null)
            {
                return null;
            }

            var appUserToUpdate = existingAppUser with { Role = newRole };

            return await _appUserRepository.UpdateAsync(id, appUserToUpdate);
        }

        public async Task<bool> DeleteAppUserAsync(int id)
        {
            return await _appUserRepository.DeleteAsync(id);
        }

        public Task<AppUser?> GetAppUserByEmailAsync(string email)
        {
            return _appUserRepository.GetByEmailAsync(email);
        }

        private void ValidateAppUserBusinessRules(AppUser appUser)
        {
            if (string.IsNullOrWhiteSpace(appUser.Name))
            {
                throw new BusinessRuleException("El nombre del usuario no puede estar vacío.");
            }

            if (string.IsNullOrWhiteSpace(appUser.LastName))
            {
                throw new BusinessRuleException("El apellido del usuario no puede estar vacío.");
            }

            if (string.IsNullOrWhiteSpace(appUser.ContactEmail) || !appUser.ContactEmail.Contains("@"))
            {
                throw new BusinessRuleException("Debe proporcionar un correo electrónico válido que contenga '@'.");
            }

            if (string.IsNullOrWhiteSpace(appUser.PhoneNumber) || appUser.PhoneNumber.Length < 8)
            {
                throw new BusinessRuleException("Debe proporcionar un número de teléfono válido de al menos 8 dígitos.");
            }

            if (appUser.Age <= 0 || appUser.Age > 120)
            {
                throw new BusinessRuleException("La edad debe estar entre 1 y 120 años.");
            }

            if (appUser.Weight <= 0)
            {
                throw new BusinessRuleException("El peso debe ser mayor a cero.");
            }
        }
    }
}