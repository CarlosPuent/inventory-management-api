using AutoMapper;
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
        private readonly IMapper _mapper;

        public AppUsersController(IAppUserService appUserService, IMapper mapper)
        {
            _appUserService = appUserService;
            _mapper = mapper;
        }

        [HttpGet("filtered")]
        public async Task<ActionResult<PagedResult<AppUserResponseDto>>> GetFilteredAppUsers([FromQuery] AppUserFilterDto filter)
        {
            var result = await _appUserService.GetFilteredAppUsersAsync(filter);
            var mappedItems = _mapper.Map<List<AppUserResponseDto>>(result.Items);
            return Ok(new PagedResult<AppUserResponseDto>(mappedItems, result.TotalCount, result.Page, result.PageSize));
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PagedResult<AppUserResponseDto>>> GetPagedAppUsers([FromQuery] PaginationRequestDto pagination)
        {
            var result = await _appUserService.GetPagedAppUsersAsync(pagination.Page, pagination.PageSize);
            var mappedItems = _mapper.Map<List<AppUserResponseDto>>(result.Items);
            return Ok(new PagedResult<AppUserResponseDto>(mappedItems, result.TotalCount, result.Page, result.PageSize));
        }

        [HttpGet]
        public async Task<ActionResult<List<AppUserResponseDto>>> GetAllAppUsers()
        {
            var appUsers = await _appUserService.GetAllAppUsersAsync();
            return Ok(_mapper.Map<List<AppUserResponseDto>>(appUsers));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUserResponseDto>> GetAppUserById(int id)
        {
            var appUser = await _appUserService.GetAppUserByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AppUserResponseDto>(appUser));
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<AppUserResponseDto>> GetAppUserByEmail(string email)
        {
            var appUser = await _appUserService.GetAppUserByEmailAsync(email);

            if (appUser == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AppUserResponseDto>(appUser));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AppUserResponseDto>> UpdateAppUser(int id, [FromBody] AppUser appUser)
        {
            var updatedAppUser = await _appUserService.UpdateAppUserAsync(id, appUser);

            if (updatedAppUser == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AppUserResponseDto>(updatedAppUser));
        }

        [HttpPatch("{id}/role")]
        public async Task<ActionResult<AppUserResponseDto>> ChangeUserRole(int id, [FromBody] ChangeRoleRequestDto request)
        {
            var updatedAppUser = await _appUserService.ChangeUserRoleAsync(id, request.Role);

            if (updatedAppUser == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AppUserResponseDto>(updatedAppUser));
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