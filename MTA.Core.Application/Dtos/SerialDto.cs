using System;

namespace MTA.Core.Application.Dtos
{
    public class SerialDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string LastLoginIp { get; set; }
    }
}