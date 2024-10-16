using devRootsApi.DTOs;
using devRootsApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace devRootsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController:ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthController (UserManager<User> userManager,
                              RoleManager<IdentityRole> roleManager,
                              IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token if successful.
        /// </summary>
        /// <param name="model">The user authentication data.</param>
        /// <returns>A JWT token if authentication is successful; otherwise, an error message.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login (UserAuthenticateDTO model)
        {
            if (ModelState.IsValid)
            {
                // Search for the user by email or username
                var user = await _userManager.FindByEmailAsync(model.EmailUsername)
                            ?? await _userManager.FindByNameAsync(model.EmailUsername);

                if (user != null)
                {
                    var result = await _userManager.CheckPasswordAsync(user, model.Password);

                    // If the password is correct
                    if (result)
                    {
                        // Assign roles
                        var userRoles = await _userManager.GetRolesAsync(user);

                        var authClaims = new List<Claim>
                        {
                            new(ClaimTypes.Name, user.UserName),
                            new(ClaimTypes.Email, user.Email),
                            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        };

                        foreach (var userRole in userRoles)
                        {
                            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                        }

                        // Creates a JWT token and returns it in the response
                        var token = GetToken(authClaims);
                        return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token) });
                    }
                    else
                    {
                        return BadRequest("Invalid password.");
                    }
                }
                else
                {
                    return NotFound("Username or Email not found.");
                }
            }

            // Model is invalid, returns an incorrect request error
            return BadRequest("Invalid request.");
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="model">The user registration data.</param>
        /// <returns>A success message if registration is successful; otherwise, an error message.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register ([FromBody] UserRegisterDTO model)
        {
            if (ModelState.IsValid)
            {
                // Check if email exists
                var emailExists = await _userManager.FindByEmailAsync(model.Email);

                if (emailExists != null)
                {
                    return BadRequest("The email is already in use.");
                }

                // Check if username exists
                var usernameExists = await _userManager.FindByNameAsync(model.Username);

                if (usernameExists != null)
                {
                    return BadRequest("The username is already in use.");
                }

                var user = new User
                {
                    Email = model.Email,
                    UserName = model.Username,
                    ProfilePicture = "https://www.gravatar.com/avatar/",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                // Try to create the user in the database
                var result = await _userManager.CreateAsync(user, model.Password);

                // If there are errors in the user creation
                if (!result.Succeeded)
                {
                    // Adds the errors to the model and returns a bad request error
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return BadRequest(ModelState);
                }

                // Each user has a default "User" role
                var role = await _roleManager.FindByNameAsync("User");

                // Assign the role to the newly created user
                result = await _userManager.AddToRoleAsync(user, role.Name);

                // If there are errors when assigning the role, it returns an incorrect request error
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return BadRequest(ModelState);
                }

                return Ok($"{user.UserName} has been successfully registered");
            }

            return BadRequest("Invalid username / password.");
        }

        /// <summary>
        /// Generates a JWT token based on provided claims.
        /// </summary>
        /// <param name="authClaims">A list of claims to include in the JWT token.</param>
        /// <returns>A JWT token.</returns>
        private JwtSecurityToken GetToken (List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddDays(7),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }
    }
}
