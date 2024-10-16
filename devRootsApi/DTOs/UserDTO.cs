using System.ComponentModel.DataAnnotations;

namespace devRootsApi.DTOs
{
    public class UserRegisterDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The username must be at least between 3 and 50 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }

    public class UserDto
    {
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The username must be at least between 3 and 50 characters")]
        public string Username { get; set; }
        
        [EmailAddress]
        public string Email { get; set; }

        public string ProfilePicture { get; set; }
    }

    public class UserPasswordDto
    {
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "RepeatedPassword is required")]
        public string RepeatedPassword { get; set; }
    }

    public class UserAuthenticateDTO
    {
        [Required(ErrorMessage = "Email or username is required")]
        public string EmailUsername { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}