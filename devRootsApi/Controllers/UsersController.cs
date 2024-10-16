using devRootsApi.Data;
using devRootsApi.DTOs;
using devRootsApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

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
                return NotFound(UserNotFound(username));
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

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="username">The current username of the user to update.</param>
        /// <param name="userDto">The updated user data.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPut("{username}")]
        public async Task<IActionResult> PutUser (string username, UserDto userDto)
        {
            // Check if the user is authorized to update
            var currentUser = User.Identity.Name;
            var isAdmin = User.IsInRole("Admin");
            if (currentUser != username && !isAdmin)
            {
                return BadRequest("You do not have the credentials to update the user.");
            }

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound(UserNotFound(username));
            }

            // Check if the new username is different and is already taken by another user
            
            if (!string.Equals(user.UserName, userDto.Username, StringComparison.OrdinalIgnoreCase))
            {
                var existingUser = await _userManager.FindByNameAsync(userDto.Username);
                
                if (existingUser != null)
                {
                    return BadRequest("The username is already taken.");
                }
                
                user.UserName = userDto.Username;
            }

            // Check if the new emial is different and is already taken by another user
            if (!string.Equals(user.Email, userDto.Email, StringComparison.OrdinalIgnoreCase))
            {
                var existingUser = await _userManager.FindByEmailAsync(userDto.Email);

                if (existingUser != null)
                {
                    return BadRequest("The email is already taken.");
                }

                user.Email = userDto.Email;
                user.NormalizedEmail = user.Email.ToUpper();
            }

            // Update other user fields
            user.ProfilePicture = userDto.ProfilePicture;
            user.UpdatedAt = DateTime.Now;

            // Update the user using UserManager
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok($"{username} updated successfully.");
        }

        /// <summary>
        /// Updates the password of an existing user.
        /// </summary>
        /// <param name="username">The current username of the user to update.</param>
        /// <param name="userDto">The updated user password data.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPut("Password/{username}")]
        public async Task<IActionResult> PutPassword (string username, UserPasswordDto userDto)
        {
            // Check if the user is authorized to update
            var currentUser = User.Identity.Name;
            var isAdmin = User.IsInRole("Admin");
            if (currentUser != username && !isAdmin)
            {
                return BadRequest("You do not have the credentials to update the user.");
            }

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound(UserNotFound(username));
            }

            if (userDto.Password != userDto.RepeatedPassword)
            {
                return BadRequest("Passwords do not match.");
            }

            // Check if the new password is different from the current password
            var currentPasswordVerification = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, userDto.Password);
            if (currentPasswordVerification == PasswordVerificationResult.Success)
            {
                return BadRequest("The new password cannot be the same as the current password.");
            }

            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, userDto.Password);
            user.UpdatedAt = DateTime.Now;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return BadRequest(updateResult.Errors);
            }

            return Ok("Password updated successfully.");
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

        /// <summary>
        /// Checks if a user exists by its username.
        /// </summary>
        /// <param name="username">The username of the user to check.</param>
        /// <returns>True if the user exists; otherwise, false.</returns>
        private bool UserExists (string username)
        {
            return _context.Users.Any(e => e.UserName == username);
        }

        /// <summary>
        /// Generates a not found message for a user.
        /// </summary>
        /// <param name="username">The username of the user not found.</param>
        /// <returns>A string message indicating the user was not found.</returns>
        private string UserNotFound (string username)
        {
            return $"The user: {username} has not been found";
        }
    }
}
