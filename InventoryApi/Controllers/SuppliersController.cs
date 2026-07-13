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
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        private readonly IMapper _mapper;

        public SuppliersController(ISupplierService supplierService, IMapper mapper)
        {
            _supplierService = supplierService;
            _mapper = mapper;
        }

        [HttpGet("filtered")]
        public async Task<ActionResult<PagedResult<SupplierResponseDto>>> GetFilteredSuppliers([FromQuery] SupplierFilterDto filter)
        {
            var result = await _supplierService.GetFilteredSuppliersAsync(filter);
            var mappedItems = _mapper.Map<List<SupplierResponseDto>>(result.Items);
            return Ok(new PagedResult<SupplierResponseDto>(mappedItems, result.TotalCount, result.Page, result.PageSize));
        }

        [HttpGet("paged")]
        public async Task<ActionResult<PagedResult<SupplierResponseDto>>> GetPagedSuppliers([FromQuery] PaginationRequestDto pagination)
        {
            var result = await _supplierService.GetPagedSuppliersAsync(pagination.Page, pagination.PageSize);
            var mappedItems = _mapper.Map<List<SupplierResponseDto>>(result.Items);
            return Ok(new PagedResult<SupplierResponseDto>(mappedItems, result.TotalCount, result.Page, result.PageSize));
        }

        [HttpGet]
        public async Task<ActionResult<List<SupplierResponseDto>>> GetAllSuppliers()
        {
            var suppliers = await _supplierService.GetAllSuppliersAsync();
            return Ok(_mapper.Map<List<SupplierResponseDto>>(suppliers));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SupplierResponseDto>> GetSupplierById(int id)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);

            if (supplier == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<SupplierResponseDto>(supplier));
        }

        [HttpPost]
        public async Task<ActionResult<SupplierResponseDto>> CreateSupplier([FromBody] Supplier supplier)
        {
            var createdSupplier = await _supplierService.CreateSupplierAsync(supplier);
            var dto = _mapper.Map<SupplierResponseDto>(createdSupplier);
            return CreatedAtAction(nameof(GetSupplierById), new { id = createdSupplier.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SupplierResponseDto>> UpdateSupplier(int id, [FromBody] Supplier supplier)
        {
            var updatedSupplier = await _supplierService.UpdateSupplierAsync(id, supplier);

            if (updatedSupplier == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<SupplierResponseDto>(updatedSupplier));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var deleted = await _supplierService.DeleteSupplierAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}