using System.ComponentModel.DataAnnotations;

namespace devRootsApi.DTOs
{
    public class BlogDTO
    {
        [Required (ErrorMessage = "Title is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The title must be at least between 3 and 50 characters")]
        public string Title { get; set; } = null!;

        [Required (ErrorMessage = "Content is required")]
        public string Content { get; set; } = null!;

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }
    }
}