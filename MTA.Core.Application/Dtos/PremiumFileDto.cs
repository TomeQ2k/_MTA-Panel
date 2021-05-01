using System;
using MTA.Core.Common.Enums;

namespace MTA.Core.Application.Dtos
{
    public class PremiumFileDto
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string Path { get; set; }
        public DateTime DateCreated { get; set; }
        public string OrderId { get; set; }
        public int UserId { get; set; }
        public PremiumFileType FileType { get; set; }
    }
}