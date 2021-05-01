using System;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Domain.Entities
{
    public class Notification : EntityModel
    {
        [Column("id", true)] public int Id { get; protected set; }
        [Column("title")] public string Text { get; protected set; }
        [Column("date")] public DateTime DateCreated { get; protected set; } = DateTime.Now;
        [Column("read")] public int Read { get; protected set; }
        [Column("userid")] public int UserId { get; protected set; }
        [Column("details")] public string Details { get; protected set; }

        public void SetText(string text) => Text = text;

        public void SetDetails(string details) => Details = details;

        public void SetUserId(int userId) => UserId = userId;

        public void MarkAsRead() => Read = 1;

        public bool IsRead => Read == 1;
    }
}