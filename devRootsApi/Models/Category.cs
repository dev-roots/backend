using devRootsApi.DTOs;
using System.ComponentModel.DataAnnotations;

namespace devRootsApi.Models
{
    public class Category
    {
        public Category () { }
        public Category (CategoryDTO category)
        {
            Title = category.Title;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<Blog> Blogs { get; set; } = [];
    }
}