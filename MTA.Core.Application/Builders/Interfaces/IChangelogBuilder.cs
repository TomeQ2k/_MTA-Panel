using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Builders.Interfaces
{
    public interface IChangelogBuilder : IBuilder<Changelog>
    {
        IChangelogBuilder SetTitle(string title);
        IChangelogBuilder SetContent(string content);
        IChangelogBuilder SetImageUrl(string imageUrl);
    }
}