﻿using System.ComponentModel.DataAnnotations;

namespace TutorialPlatform.Models
{
    public class Tutorial
    {
        public int Id { get; set; }
        [Required] [MaxLength(200)] public string Title { get; set; } = string.Empty;
        [MaxLength(1000)] public string Description { get; set; } = string.Empty;
        public ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();
    }
}