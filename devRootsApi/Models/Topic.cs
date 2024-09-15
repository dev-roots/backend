using devRootsApi.DTOs;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace devRootsApi.Models
{
    public class Topic
    {
        public Topic() { }

        public Topic(TopicDTO topic)
        {
            this.Title = topic.Title;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }    

        public string Title { get; set; }

        public virtual Collection<Subtopic> Subtopics { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
