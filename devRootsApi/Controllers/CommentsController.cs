using devRootsApi.Data;
using devRootsApi.DTOs;
using devRootsApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace devRootsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController:ControllerBase
    {
        private readonly DevRootContext _context;

        public CommentsController (DevRootContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all comments.
        /// </summary>
        /// <returns>A list of comments.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments ()
        {
            var comments = await _context.Comments.Include(b => b.User)
                                                  .Include(b => b.Blog)
                                                  .ToListAsync();

            return Ok(comments);
        }

        /// <summary>
        /// Gets a specific comment by ID.
        /// </summary>
        /// <param name="id">The ID of the comment to retrieve.</param>
        /// <returns>The requested comment.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment (int id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound(CommentNotFound(id));
            }

            return comment;
        }

        /// <summary>
        /// Updates an existing comment.
        /// </summary>
        /// <param name="id">The ID of the comment to update.</param>
        /// <param name="commentDTO">The updated comment data.</param>
        /// <returns>The updated comment.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment (int id, CommentDTO commentDTO)
        {
            if (!await CheckCommentAsync(commentDTO))
            {
                return BadRequest("The user or blog does not exist");
            }

            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound(CommentNotFound(id));
            }

            UpdateComment(comment, commentDTO);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!CommentExists(id))
            {
                return NotFound(CommentNotFound(id));
            }

            return Ok(comment);
        }

        /// <summary>
        /// Creates a new comment.
        /// </summary>
        /// <param name="commentDTO">The comment data.</param>
        /// <returns>The created comment.</returns>
        [HttpPost]
        public async Task<ActionResult<Comment>> PostComment (CommentDTO commentDTO)
        {
            if (!await CheckCommentAsync(commentDTO))
            {
                return BadRequest("The user or blog does not exist");
            }

            var comment = new Comment(commentDTO);

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComment", new { id = comment.Id }, comment);
        }

        /// <summary>
        /// Deletes a specific comment by ID.
        /// </summary>
        /// <param name="id">The ID of the comment to delete.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment (int id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound(CommentNotFound(id));
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Checks if a comment exists by ID.
        /// </summary>
        /// <param name="id">The ID of the comment.</param>
        /// <returns>True if the comment exists, false otherwise.</returns>
        private bool CommentExists (int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }

        // ****************************************************************************************************
        // Helper methods
        // ****************************************************************************************************

        /// <summary>
        /// Returns a not found message for a comment.
        /// </summary>
        /// <param name="id">The ID of the comment.</param>
        /// <returns>A not found message.</returns>
        private string CommentNotFound (int id)
        {
            return $"The comment with id: {id} has not been found";
        }

        /// <summary>
        /// Updates a comment with new data.
        /// </summary>
        /// <param name="comment">The comment to update.</param>
        /// <param name="commentDTO">The new comment data.</param>
        private void UpdateComment (Comment comment, CommentDTO commentDTO)
        {
            comment.Username = commentDTO.Username;
            comment.BlogId = commentDTO.BlogId;
            comment.Content = commentDTO.Content;
            comment.UpdatedAt = DateTime.Now;
        }

        /// <summary>
        /// Checks if the user and blog exist for a comment.
        /// </summary>
        /// <param name="commentDTO">The comment data.</param>
        /// <returns>True if both the user and blog exist, false otherwise.</returns>
        private async Task<bool> CheckCommentAsync (CommentDTO commentDTO)
        {
            var userExists = await _context.Users.AnyAsync(o => o.UserName == commentDTO.Username);

            if (!userExists)
            {
                return false;
            }

            var blogExists = await _context.Blogs.AnyAsync(o => o.Id == commentDTO.BlogId);

            if (!blogExists)
            {
                return false;
            }

            return true;
        }
    }
}
