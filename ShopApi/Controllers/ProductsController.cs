using Microsoft.AspNetCore.Mvc;
using ShopApi.Models;
using ShopApi.Services;

namespace ShopApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_productService.GetAllProducts());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            try
            {
                var createdProduct = _productService.CreateProduct(product);
                return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Product product)
        {
            try
            {
                var updatedProduct = _productService.UpdateProduct(id, product);
                return Ok(updatedProduct);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _productService.DeleteProduct(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("category/{categoryId}")]
        public IActionResult GetByCategory(int categoryId)
        {
            return Ok(_productService.GetProductsByCategory(categoryId));
        }
    }
}
