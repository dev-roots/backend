using System.ComponentModel.DataAnnotations;

namespace devRootsApi.DTOs
{
    public class CommentDTO
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Blog is required")]
        public int BlogId { get; set; }

        [Required(ErrorMessage = "Content is required")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "The content must be at least between 2 and 255 characters")]
        public string Content { get; set; }
    }
}