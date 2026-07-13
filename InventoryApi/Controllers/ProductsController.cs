using AutoMapper;
using InventoryApi.DTOs;
using InventoryApi.Models;
using InventoryApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IMapper _mapper;

    public ProductsController(IProductService productService, IMapper mapper)
    {
        _productService = productService;
        _mapper = mapper;
    }

    [HttpGet("filtered")]
    public async Task<ActionResult<PagedResult<ProductResponseDto>>> GetFilteredProducts([FromQuery] ProductFilterDto filter)
    {
        var result = await _productService.GetFilteredProductsAsync(filter);
        var mappedItems = _mapper.Map<List<ProductResponseDto>>(result.Items);
        return Ok(new PagedResult<ProductResponseDto>(mappedItems, result.TotalCount, result.Page, result.PageSize));
    }

    [HttpGet("paged")]
    public async Task<ActionResult<PagedResult<ProductResponseDto>>> GetPagedProducts([FromQuery] PaginationRequestDto pagination)
    {
        var result = await _productService.GetPagedProductsAsync(pagination.Page, pagination.PageSize);
        var mappedItems = _mapper.Map<List<ProductResponseDto>>(result.Items);
        return Ok(new PagedResult<ProductResponseDto>(mappedItems, result.TotalCount, result.Page, result.PageSize));
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductResponseDto>>> GetAllProducts()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(_mapper.Map<List<ProductResponseDto>>(products));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResponseDto>> GetProductById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<ProductResponseDto>(product));
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponseDto>> CreateProduct([FromBody] Product product)
    {
        var createdProduct = await _productService.CreateProductAsync(product);
        var dto = _mapper.Map<ProductResponseDto>(createdProduct);
        return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, dto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductResponseDto>> UpdateProduct(int id, [FromBody] Product product)
    {
        var updatedProduct = await _productService.UpdateProductAsync(id, product);

        if (updatedProduct == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<ProductResponseDto>(updatedProduct));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var deleted = await _productService.DeleteProductAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}