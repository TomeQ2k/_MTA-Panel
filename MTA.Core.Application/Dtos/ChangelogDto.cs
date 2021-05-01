using System;

namespace MTA.Core.Application.Dtos
{
    public class ChangelogDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public string ImageUrl { get; set; }
    }
}