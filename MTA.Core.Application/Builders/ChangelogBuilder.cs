using MTA.Core.Application.Builders.Interfaces;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Builders
{
    public class ChangelogBuilder : IChangelogBuilder
    {
        private readonly Changelog changelog = new Changelog();

        public IChangelogBuilder SetTitle(string title)
        {
            changelog.SetTitle(title);
            return this;
        }

        public IChangelogBuilder SetContent(string content)
        {
            changelog.SetContent(content);
            return this;
        }

        public IChangelogBuilder SetImageUrl(string imageUrl)
        {
            changelog.SetImageUrl(imageUrl);
            return this;
        }

        public Changelog Build() => this.changelog;
    }
}