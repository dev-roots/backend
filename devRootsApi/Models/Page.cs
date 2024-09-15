using devRootsApi.DTOs;
using System.ComponentModel.DataAnnotations;

namespace devRootsApi.Models
{
    public class Page
    {
        public Page() { }

        public Page(PageDTO page)
        {
            this.Title = page.Title;
            this.SubtopicId = page.SubtopicId;
            this.Content = page.Content;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
        public int SubtopicId { get; set; }
        public virtual Subtopic Subtopic { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
