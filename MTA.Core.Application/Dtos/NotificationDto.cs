using System;

namespace MTA.Core.Application.Dtos
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsRead { get; set; }
        public int UserId { get; set; }
        public string Details { get; set; }
    }
}