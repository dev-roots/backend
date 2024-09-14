using Microsoft.AspNetCore.Identity;

namespace devRootsApi.Models
{
    public class User:IdentityUser
    {
        public string ProfilePicture { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } = [];

        public virtual ICollection<Blog> Blogs { get; set; } = [];
    }

    public class UserFields
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ProfilePicture { get; set; }

        public List<string> Roles { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } = [];

        public virtual ICollection<Blog> Blogs { get; set; } = [];
    }
}