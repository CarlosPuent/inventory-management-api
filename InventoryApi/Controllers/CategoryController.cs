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

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PagedResult<Category>>> GetPagedCategories([FromQuery] PaginationRequestDto pagination)
        {
            var result = await _categoryService.GetPagedCategoriesAsync(pagination.Page, pagination.PageSize);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory([FromBody] Category category)
        {
            var createdCategory = await _categoryService.CreateCategoryAsync(category);
            return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, createdCategory);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Category>> UpdateCategory(int id, [FromBody] Category category)
        {
            var updatedCategory = await _categoryService.UpdateCategoryAsync(id, category);

            if (updatedCategory == null)
            {
                return NotFound();
            }

            return Ok(updatedCategory);
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