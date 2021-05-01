using System;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Domain.Entities
{
    public abstract class BaseFile : EntityModel
    {
        [Column("id", true)] public string Id { get; protected set; } = Utils.Id();
        [Column("url")] public string Url { get; protected set; }
        [Column("path")] public string Path { get; protected set; }
        [Column("dateCreated")] public DateTime DateCreated { get; protected set; } = DateTime.Now;

        public static T Create<T>(string url, string path) where T : BaseFile, new() => new T
        {
            Url = url,
            Path = path
        };
    }
}