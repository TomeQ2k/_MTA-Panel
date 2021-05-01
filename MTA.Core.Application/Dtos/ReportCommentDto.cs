using System;

namespace MTA.Core.Application.Dtos
{
    public class ReportCommentDto
    {
        public string Id { get; set; }
        public string ReportId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsPrivate { get; set; }
    }
}