using InventoryApi.Models;
using InventoryApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApi.Controllers
{
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

        [HttpPost]
        public async Task<ActionResult<AppUser>> CreateAppUser([FromBody] AppUser appUser)
        {
            try
            {
                var createdAppUser = await _appUserService.CreateAppUserAsync(appUser);
                return CreatedAtAction(nameof(GetAppUserById), new { id = createdAppUser.Id }, createdAppUser);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}