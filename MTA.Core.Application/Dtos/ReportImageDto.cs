using System;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Dtos
{
    public class ReportImageDto : EntityModel
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public DateTime DateCreated { get; set; }
        public string ReportId { get; set; }
        public int UserId { get; set; }
        public long Size { get; set; }
    }
}