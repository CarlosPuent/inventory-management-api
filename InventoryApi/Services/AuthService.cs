using InventoryApi.DTOs;
using InventoryApi.Models;
using InventoryApi.Repositories;
using Microsoft.AspNetCore.Identity;

namespace InventoryApi.Services;

public class AuthService : IAuthService
{
    private readonly IAppUserRepository _appUserRepository;
    private readonly IPasswordHasher<AppUser> _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthService(
        IAppUserRepository appUserRepository,
        IPasswordHasher<AppUser> passwordHasher,
        ITokenService tokenService)
    {
        _appUserRepository = appUserRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        var newAppUser = new AppUser(
            Id: 0,
            Name: request.Name,
            LastName: request.LastName,
            ContactEmail: request.ContactEmail,
            PhoneNumber: request.PhoneNumber,
            Age: request.Age,
            Weight: request.Weight,
            IsActive: true,
            PasswordHash: "",
            Role: "User"
        );

        var hashedPassword = _passwordHasher.HashPassword(newAppUser, request.Password);
        var appUserToCreate = newAppUser with { PasswordHash = hashedPassword };

        var createdAppUser = await _appUserRepository.AddAsync(appUserToCreate);
        var token = _tokenService.GenerateToken(createdAppUser);

        return new AuthResponseDto(token, createdAppUser.Id, createdAppUser.Name, createdAppUser.Role);
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto request)
    {
        var appUser = await _appUserRepository.GetByEmailAsync(request.ContactEmail);

        if (appUser == null)
        {
            return null;
        }

        var result = _passwordHasher.VerifyHashedPassword(appUser, appUser.PasswordHash, request.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            return null;
        }

        var token = _tokenService.GenerateToken(appUser);

        return new AuthResponseDto(token, appUser.Id, appUser.Name, appUser.Role);
    }
}