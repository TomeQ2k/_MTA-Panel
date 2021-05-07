using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Domain.Entities
{
    public class ChangelogImage : BaseFile
    {
        [Column("changelogId")] public string ChangelogId { get; protected set; }

        public static ChangelogImage Create(string path, string changelogId) => new()
        {
            Path = path,
            ChangelogId = changelogId
        };
    }
}