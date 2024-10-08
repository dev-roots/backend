﻿using devRootsApi.DTOs;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace devRootsApi.Models
{
    public class Subtopic
    {
        public Subtopic() { }

        public Subtopic(SubtopicDTO subtopic)
        {
            Title = subtopic.Title;
            TopicId = subtopic.TopicId;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        public int TopicId { get; set; }
        public virtual Topic Topic { get; set; }

        public string Title { get; set; }

        public virtual Collection<Page> Pages { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
