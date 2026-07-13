using InventoryApi.DTOs;
using InventoryApi.Models;
using InventoryApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AppUsersController : ControllerBase
    {
        private readonly IAppUserService _appUserService;

        public AppUsersController(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }

        [HttpGet]
        public async Task<ActionResult<List<AppUser>>> GetAllAppUsers()
        {
            var appUsers = await _appUserService.GetAllAppUsersAsync();
            return Ok(appUsers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetAppUserById(int id)
        {
            var appUser = await _appUserService.GetAppUserByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            return Ok(appUser);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AppUser>> UpdateAppUser(int id, [FromBody] AppUser appUser)
        {
            var updatedAppUser = await _appUserService.UpdateAppUserAsync(id, appUser);

            if (updatedAppUser == null)
            {
                return NotFound();
            }

            return Ok(updatedAppUser);
        }

        [HttpPatch("{id}/role")]
        public async Task<ActionResult<AppUser>> ChangeUserRole(int id, [FromBody] ChangeRoleRequestDto request)
        {
            var updatedAppUser = await _appUserService.ChangeUserRoleAsync(id, request.Role);

            if (updatedAppUser == null)
            {
                return NotFound();
            }

            return Ok(updatedAppUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppUser(int id)
        {
            var deleted = await _appUserService.DeleteAppUserAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}