using devRootsApi.DTOs;
using System.ComponentModel.DataAnnotations;

namespace devRootsApi.Models
{
    public class Comment
    {
        public Comment () { }

        public Comment (CommentDTO comment)
        {
            Username = comment.Username;
            BlogId = comment.BlogId;
            Content = comment.Content;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        public int? ParentCommentId { get; set; }

        public virtual Comment? ParentComment { get; set; }

        public string Username { get; set; }

        public virtual User User { get; set; } = null!;

        public int BlogId { get; set; }

        public virtual Blog Blog { get; set; } = null!;

        public string Content { get; set; }

        public virtual ICollection<Comment> Replies { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}