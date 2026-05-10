using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyExpenses.Dtos;
using MyExpenses.Models;
using MyExpenses.Services;

namespace MyExpenses.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoriesService categoriesService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetCategories()
        {
            return Ok(await categoriesService.GetAllCategoriesAsync());
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Category?>> GetCategoryById(int id)
        {
            var category = await categoriesService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound($"Category with id {id} not found");
            }
            
            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult> CreateCategory(CreateCategoryRequest? createCategoryRequest)
        {
            if (createCategoryRequest == null)
            {
                return BadRequest("Request body cannot be null");  
            }
            
            var category = await categoriesService.CreateCategoryAsync(createCategoryRequest);
            return CreatedAtAction(nameof(GetCategoryById), new {id = category.Id}, category);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(int id, UpdateCategoryRequest? updateCategoryRequest)
        {
            if (updateCategoryRequest == null)
            {
                return BadRequest("Request body cannot be null");  
            }
            
            var category = await categoriesService.UpdateCategoryAsync(id, updateCategoryRequest);

            if (category == null)
            {
                return NotFound($"Category with id {id} not found"); 
            }
            
            return Ok(category);
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var deleted = await categoriesService.DeleteCategoryAsync(id);
            return deleted ? Ok($"Category with id {id} is deleted") : NotFound($"Category with id {id} not found");
        }      
    }
}
