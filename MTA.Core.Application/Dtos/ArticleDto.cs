using System;

namespace MTA.Core.Application.Dtos
{
    public class ArticleDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public string ImageUrl { get; set; }
        public int Category { get; set; }
    }
}