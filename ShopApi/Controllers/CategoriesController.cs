using Microsoft.AspNetCore.Mvc;
using ShopApi.Models;
using ShopApi.Services;

namespace ShopApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_categoryService.GetAllCategories());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            try
            {
                var createdCategory = _categoryService.CreateCategory(category);
                return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id }, createdCategory);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Category category)
        {
            try
            {
                var updatedCategory = _categoryService.UpdateCategory(id, category);
                return Ok(updatedCategory);
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
        public IActionResult Delete(int id, [FromQuery] int? moveToCategoryId)
        {
            try
            {
                _categoryService.DeleteCategory(id, moveToCategoryId);
                return NoContent();
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
    }
}
