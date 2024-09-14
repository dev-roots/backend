using devRootsApi.DTOs;
using System.ComponentModel.DataAnnotations;

namespace devRootsApi.Models
{
    public class Blog
    {
        public Blog () { }

        public Blog (BlogDTO blog)
        {
            Title = blog.Title;
            Content = blog.Content;
            Username = blog.Username;
            CategoryId = blog.CategoryId;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public string Username { get; set; }

        public virtual User User { get; set; } = null!;

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; } = null!;

        public virtual ICollection<Comment> Comments { get; set; } = [];

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}