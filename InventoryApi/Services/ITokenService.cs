using InventoryApi.Models;

namespace InventoryApi.Services;

public interface ITokenService
{
    string GenerateToken(AppUser appUser);
}