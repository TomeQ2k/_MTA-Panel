using System;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Domain.Entities
{
    public abstract class BaseFile : EntityModel
    {
        [Column("id", true)] public string Id { get; protected set; } = Utils.Id();
        [Column("path")] public string Path { get; protected set; }
        [Column("dateCreated")] public DateTime DateCreated { get; protected set; } = DateTime.Now;

        public static T Create<T>(string path) where T : BaseFile, new() => new T
        {
            Path = path
        };
    }
}