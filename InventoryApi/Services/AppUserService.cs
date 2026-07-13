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

        public async Task<AppUser?> UpdateAppUserAsync(int id, AppUser appUser)
        {
            var existingAppUser = await _appUserRepository.GetByIdAsync(id);

            if (existingAppUser == null)
            {
                return null;
            }

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
    }
}