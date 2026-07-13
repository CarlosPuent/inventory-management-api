using AutoMapper;
using InventoryApi.DTOs;
using InventoryApi.Models;
using InventoryApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet("filtered")]
        public async Task<ActionResult<PagedResult<CategoryResponseDto>>> GetFilteredCategories([FromQuery] CategoryFilterDto filter)
        {
            var result = await _categoryService.GetFilteredCategoriesAsync(filter);
            var mappedItems = _mapper.Map<List<CategoryResponseDto>>(result.Items);
            return Ok(new PagedResult<CategoryResponseDto>(mappedItems, result.TotalCount, result.Page, result.PageSize));
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PagedResult<CategoryResponseDto>>> GetPagedCategories([FromQuery] PaginationRequestDto pagination)
        {
            var result = await _categoryService.GetPagedCategoriesAsync(pagination.Page, pagination.PageSize);
            var mappedItems = _mapper.Map<List<CategoryResponseDto>>(result.Items);
            return Ok(new PagedResult<CategoryResponseDto>(mappedItems, result.TotalCount, result.Page, result.PageSize));
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryResponseDto>>> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(_mapper.Map<List<CategoryResponseDto>>(categories));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryResponseDto>> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CategoryResponseDto>(category));
        }

        [HttpPost]
        public async Task<ActionResult<CategoryResponseDto>> CreateCategory([FromBody] Category category)
        {
            var createdCategory = await _categoryService.CreateCategoryAsync(category);
            var dto = _mapper.Map<CategoryResponseDto>(createdCategory);
            return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryResponseDto>> UpdateCategory(int id, [FromBody] Category category)
        {
            var updatedCategory = await _categoryService.UpdateCategoryAsync(id, category);

            if (updatedCategory == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CategoryResponseDto>(updatedCategory));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var deleted = await _categoryService.DeleteCategoryAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}