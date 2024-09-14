using devRootsApi.Data;
using devRootsApi.DTOs;
using devRootsApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace devRootsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController:ControllerBase
    {
        private readonly DevRootContext _context;

        public CategoriesController (DevRootContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        /// <returns>A list of Category objects.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories ()
        {
            return await _context.Categories.ToListAsync();
        }

        /// <summary>
        /// Retrieves a category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category to retrieve.</param>
        /// <returns>A Category object if found; otherwise, a NotFound result.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory (int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound(CategoryNotFound(id));
            }

            return category;
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="id">The ID of the category to update.</param>
        /// <param name="categoryDTO">The updated category data.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory (int id, CategoryDTO categoryDTO)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound(CategoryNotFound(id));
            }

            UpdateCategory(category, categoryDTO);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!CategoryExists(id))
            {
                return NotFound(CategoryNotFound(id));
            }

            return Ok(category);
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="categoryDTO">The data for the new category.</param>
        /// <returns>The created Category object.</returns>
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory (CategoryDTO categoryDTO)
        {
            var category = new Category(categoryDTO);

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = category.Id }, category);
        }

        /// <summary>
        /// Deletes a category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category to delete.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory (int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound(CategoryNotFound(id));
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Checks if a category exists by its ID.
        /// </summary>
        /// <param name="id">The ID of the category to check.</param>
        /// <returns>True if the category exists; otherwise, false.</returns>
        private bool CategoryExists (int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }

        // ****************************************************************************************************
        // Helper methods
        // ****************************************************************************************************

        /// <summary>
        /// Generates a not found message for a category.
        /// </summary>
        /// <param name="id">The ID of the category not found.</param>
        /// <returns>A string message indicating the category was not found.</returns>
        private string CategoryNotFound (int id)
        {
            return $"The category with id: {id} has not been found";
        }

        /// <summary>
        /// Updates a category entity with data from a CategoryDTO.
        /// </summary>
        /// <param name="category">The category entity to update.</param>
        /// <param name="categoryDTO">The category data to update.</param>
        private void UpdateCategory (Category category, CategoryDTO categoryDTO)
        {
            category.Title = categoryDTO.Title;
            category.UpdatedAt = DateTime.Now;
        }
    }
}
