using devRootsApi.Data;
using devRootsApi.DTOs;
using devRootsApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace devRootsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController:ControllerBase
    {
        private readonly DevRootContext _context;

        public BlogsController (DevRootContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all blogs.
        /// </summary>
        /// <returns>A list of blogs.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Blog>>> GetBlogs ()
        {
            var blogs = await _context.Blogs.Include(b => b.User)
                                            .Include(b => b.Category)
                                            .Include(b => b.Comments)
                                            .ToListAsync();

            return Ok(blogs);
        }

        /// <summary>
        /// Gets a specific blog by ID.
        /// </summary>
        /// <param name="id">The ID of the blog to retrieve.</param>
        /// <returns>The requested blog.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Blog>> GetBlog (int id)
        {
            var blog = await _context.Blogs.Include(b => b.User)
                                           .Include(b => b.Category)
                                           .Include(b => b.Comments)
                                           .FirstOrDefaultAsync(b => b.Id == id);

            if (blog == null)
            {
                return NotFound(BlogNotFound(id));
            }

            return blog;
        }

        /// <summary>
        /// Updates an existing blog.
        /// </summary>
        /// <param name="id">The ID of the blog to update.</param>
        /// <param name="blogDTO">The updated blog data.</param>
        /// <returns>The updated blog.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBlog (int id, BlogDTO blogDTO)
        {
            if (!await CheckBlogAsync(blogDTO))
            {
                return BadRequest("The user or category does not exist");
            }

            var blog = await _context.Blogs.FindAsync(id);

            if (blog == null)
            {
                return NotFound(BlogNotFound(id));
            }

            UpdateBlog(blog, blogDTO);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!BlogExists(id))
            {
                return NotFound(BlogNotFound(id));
            }

            return Ok(blog);
        }

        /// <summary>
        /// Creates a new blog.
        /// </summary>
        /// <param name="blogDTO">The blog data.</param>
        /// <returns>The created blog.</returns>
        [HttpPost]
        public async Task<ActionResult<Blog>> PostBlog (BlogDTO blogDTO)
        {
            if (!await CheckBlogAsync(blogDTO))
            {
                return BadRequest("The user or category does not exist");
            }

            var blog = new Blog(blogDTO);

            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBlog", new { id = blog.Id }, blog);
        }

        /// <summary>
        /// Deletes a specific blog by ID.
        /// </summary>
        /// <param name="id">The ID of the blog to delete.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog (int id)
        {
            var blog = await _context.Blogs.FindAsync(id);

            if (blog == null)
            {
                return NotFound(BlogNotFound(id));
            }

            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Checks if a blog exists by ID.
        /// </summary>
        /// <param name="id">The ID of the blog.</param>
        /// <returns>True if the blog exists, false otherwise.</returns>
        private bool BlogExists (int id)
        {
            return _context.Blogs.Any(e => e.Id == id);
        }

        // ****************************************************************************************************
        // Helper methods
        // ****************************************************************************************************

        /// <summary>
        /// Returns a not found message for a blog.
        /// </summary>
        /// <param name="id">The ID of the blog.</param>
        /// <returns>A not found message.</returns>
        private string BlogNotFound (int id)
        {
            return $"The blog with id: {id} has not been found";
        }

        /// <summary>
        /// Updates a blog with new data.
        /// </summary>
        /// <param name="blog">The blog to update.</param>
        /// <param name="blogDTO">The new blog data.</param>
        private void UpdateBlog (Blog blog, BlogDTO blogDTO)
        {
            blog.Title = blogDTO.Title;
            blog.Content = blogDTO.Content;
            blog.Username = blogDTO.Username;
            blog.CategoryId = blogDTO.CategoryId;
            blog.UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Checks if the user and category exist for a blog.
        /// </summary>
        /// <param name="blogDTO">The blog data.</param>
        /// <returns>True if both the user and category exist, false otherwise.</returns>
        private async Task<bool> CheckBlogAsync (BlogDTO blogDTO)
        {
            var userExists = await _context.Users.AnyAsync(o => o.UserName == blogDTO.Username);

            if (!userExists)
            {
                return false;
            }

            var categoryExists = await _context.Categories.AnyAsync(o => o.Id == blogDTO.CategoryId);

            if (!categoryExists)
            {
                return false;
            }

            return true;
        }
    }
}
