using System;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Domain.Entities
{
    public class Changelog : EntityModel
    {
        [Column("id", true)] public string Id { get; protected set; } = Utils.Id();
        [Column("title")] public string Title { get; protected set; }
        [Column("content")] public string Content { get; protected set; }
        [Column("dateCreated")] public DateTime DateCreated { get; protected set; } = DateTime.Now;
        [Column("imageUrl")] public string ImageUrl { get; protected set; }

        public static Changelog Create(string title, string imageUrl) => new Changelog()
        {
            Title = title,
            ImageUrl = imageUrl
        };

        public void SetTitle(string title) => Title = title;
        public void SetContent(string content) => Content = content;
        public void SetImageUrl(string imageUrl) => ImageUrl = imageUrl;
    }
}