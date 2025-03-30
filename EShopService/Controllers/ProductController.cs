using EShop.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EShop.Domain.Models;

namespace EShopService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Product>> Get()
    {
        var products = _productService.GetAll();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public ActionResult<Product> Get(int id)
    {
        var product = _productService.GetById(id);
        if (product == null)
            return NotFound();
        return Ok(product);
    }

    [HttpPost]
    public IActionResult Post([FromBody] Product product)
    {
        _productService.Add(product);
        return CreatedAtAction(nameof(Get), new { id = product.id }, product);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] Product updatedProduct)
    {
        var existing = _productService.GetById(id);
        if (existing == null)
            return NotFound();

        updatedProduct.id = id; 
        _productService.Update(updatedProduct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var product = _productService.GetById(id);
        if (product == null)
            return NotFound();

        _productService.Delete(id);
        return NoContent();
    }
}