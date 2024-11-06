using Management.Api.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    // Tüm ürünleri getirme (Sadece Admin ve SuperAdmin erişebilir)
    [HttpGet("get-products")]
    [Authorize]
    [UserTypeAuthorize(UserType.Admin, UserType.Superadmin)]
    public async Task<IActionResult> GetAllProducts([FromQuery] bool includeDeleted = false)
    {
        var products = await _productService.GetAllProductsAsync(includeDeleted);
        return Ok(products);
    }

    // Ürünü ID ile getirme (Sadece Admin ve SuperAdmin erişebilir)
    [HttpGet("{id}/get-product-by-id")]
    [Authorize]
    [UserTypeAuthorize(UserType.Admin, UserType.Superadmin)]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
            return NotFound(new { message = "Product not found" });

        return Ok(product);
    }

    // Yeni ürün oluşturma (Sadece Admin ve SuperAdmin erişebilir)
    [HttpPost("create-product")]
    [Authorize]
    [UserTypeAuthorize(UserType.Admin, UserType.Superadmin)]
    public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto productDto)
    {
        var product = await _productService.AddProductAsync(productDto);
        return Ok(new { message = "Product created successfully", product });
    }

    // Ürün güncelleme (Sadece Admin ve SuperAdmin tüm ürünleri güncelleyebilir)
    [HttpPut("{id}/update-product")]
    [Authorize]
    [UserTypeAuthorize(UserType.Admin, UserType.Superadmin)]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductUpdateDto productDto)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
            return NotFound(new { message = "Product not found" });

        product.Name = productDto.Name;
        product.ProductCode = productDto.ProductCode;
        product.Price = productDto.Price;
        product.ProductImage = productDto.ProductImage;

        var updated = await _productService.UpdateProductAsync(product);
        if (updated)
            return Ok(new { message = "Product updated successfully" });

        return BadRequest(new { message = "Product update failed" });
    }

    // Ürünü soft delete yapma (Sadece Admin ve SuperAdmin erişebilir)
    [HttpDelete("{id}/delete-product")]
    [Authorize]
    [UserTypeAuthorize(UserType.Admin, UserType.Superadmin)]
    public async Task<IActionResult> SoftDeleteProduct(Guid id)
    {
        var deleted = await _productService.DeleteProductAsync(id);
        if (deleted)
            return Ok(new { message = "Product soft-deleted successfully" });

        return BadRequest(new { message = "Product deletion failed" });
    }
}
