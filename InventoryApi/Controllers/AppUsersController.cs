using InventoryApi.Models;
using InventoryApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppUsersController : ControllerBase
    {
        private readonly IAppUsersService _appUsersService;

        public AppUsersController(IAppUsersService appUsersService)
        { 
            _appUsersService = appUsersService;
        }

        [HttpGet]
        public async Task<ActionResult<List<AppUsers>>> GetAllAppUsers()
        {
            var appUsers = await _appUsersService.GetAllAppUsersAsync();
            return Ok(appUsers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUsers>> GetAppUsersById(int id)
        {
            var appUsers = await _appUsersService.GetAppUserByIdAsync(id);

            if (appUsers == null)
            {
                return NotFound();
            }

            return Ok(appUsers);
        }

        [HttpPost]
        public async Task<ActionResult<AppUsers>> CreateAppUsers([FromBody] AppUsers appUsers)
        {
            try
            {
                var createdAppUsers = await _appUsersService.CreateAppUserAsync(appUsers);
                return CreatedAtAction(nameof(GetAppUsersById), new { id = createdAppUsers.Id }, createdAppUsers);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
