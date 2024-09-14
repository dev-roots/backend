using devRootsApi.Data;
using devRootsApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace devRootsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController:ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly DevRootContext _context;

        public UsersController (UserManager<User> userManager, DevRootContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of all users with their associated comments, blogs, and roles.
        /// </summary>
        /// <returns>A list of UserFields objects containing user details, comments, blogs, and roles.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserFields>>> GetUsers ()
        {
            var users = await _userManager.Users
                .Select(a => new UserFields
                {
                    Username = a.UserName,
                    Email = a.Email,
                    ProfilePicture = a.ProfilePicture,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt
                })
                .ToListAsync();

            foreach (var user in users)
            {
                user.Comments = await GetUserCommentsAsync(user.Username);
                user.Blogs = await GetUserBlogsAsync(user.Username);

                var identityUser = await _userManager.FindByNameAsync(user.Username);
                var roles = await _userManager.GetRolesAsync(identityUser);
                user.Roles = roles.ToList();
            }

            return Ok(users);
        }

        /// <summary>
        /// Retrieves details of a user by username.
        /// </summary>
        /// <param name="username">The username of the user to retrieve.</param>
        /// <returns>A UserFields object containing user details, comments, blogs, and roles.</returns>
        [HttpGet("{username}")]
        public async Task<ActionResult<UserFields>> GetUser (string username)
        {
            var userManager = await _userManager.FindByNameAsync(username);

            if (userManager == null)
            {
                return NotFound($"The user {username} has not been found.");
            }

            var user = new UserFields
            {
                Username = userManager.UserName,
                Email = userManager.Email,
                ProfilePicture = userManager.ProfilePicture,
                CreatedAt = userManager.CreatedAt,
                UpdatedAt = userManager.UpdatedAt,
                Comments = await GetUserCommentsAsync(username),
                Blogs = await GetUserBlogsAsync(username)
            };

            var roles = await _userManager.GetRolesAsync(userManager);
            user.Roles = roles.ToList();

            return Ok(user);
        }

        // ****************************************************************************************************
        // Helper methods
        // ****************************************************************************************************

        /// <summary>
        /// Retrieves a list of comments made by a specific user.
        /// </summary>
        /// <param name="username">The username of the user whose comments are to be retrieved.</param>
        /// <returns>A list of Comment objects made by the user.</returns>
        private async Task<List<Comment>> GetUserCommentsAsync (string username)
        {
            return await _context.Comments
                .Where(i => i.Username == username)
                .Select(c => new Comment
                {
                    Id = c.Id,
                    ParentCommentId = c.ParentCommentId,
                    Username = c.Username,
                    BlogId = c.BlogId,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                })
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a list of blogs created by a specific user.
        /// </summary>
        /// <param name="username">The username of the user whose blogs are to be retrieved.</param>
        /// <returns>A list of Blog objects created by the user.</returns>
        private async Task<List<Blog>> GetUserBlogsAsync (string username)
        {
            return await _context.Blogs
                .Where(i => i.Username == username)
                .Select(b => new Blog
                {
                    Id = b.Id,
                    Title = b.Title,
                    Content = b.Content,
                    Username = b.Username,
                    CategoryId = b.CategoryId,
                    CreatedAt = b.CreatedAt,
                    UpdatedAt = b.UpdatedAt
                })
                .ToListAsync();
        }
    }
}
