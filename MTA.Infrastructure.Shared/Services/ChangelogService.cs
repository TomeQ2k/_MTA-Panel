using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using MTA.Core.Application.Builders;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Services;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Shared.Services
{
    public class ChangelogService : IChangelogService
    {
        private readonly IDatabase database;
        private readonly IFilesManager filesManager;
        private readonly IMapper mapper;

        public ChangelogService(IDatabase database, IFilesManager filesManager, IMapper mapper)
        {
            this.database = database;
            this.filesManager = filesManager;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<Changelog>> GetChangelogs()
            => await database.ChangelogRepository.GetAll();

        public async Task<Changelog> CreateChangelog(CreateChangelogRequest request)
        {
            var changelog = new ChangelogBuilder()
                .SetTitle(request.Title)
                .SetContent(request.Content)
                .Build();

            if (!await database.ChangelogRepository.Insert(changelog, false))
                throw new DatabaseException();

            if (request.Image != null)
            {
                await UploadImage(request.Image, changelog);
                await database.ChangelogRepository.Update(changelog);
            }

            return changelog;
        }

        public async Task<Changelog> UpdateChangelog(UpdateChangelogRequest request)
        {
            var changelog = await database.ChangelogRepository.FindById(request.ChangelogId)
                            ?? throw new EntityNotFoundException("Changelog not found");

            changelog = mapper.Map(request, changelog);

            if (request.IsImageDeleted && !string.IsNullOrEmpty(changelog.ImageUrl))
                await DeleteImage(changelog);
            else if (!request.IsImageDeleted && request.Image != null)
            {
                await DeleteImage(changelog);
                await UploadImage(request.Image, changelog);
            }

            return await database.ChangelogRepository.Update(changelog) ? changelog : throw new DatabaseException();
        }

        public async Task<bool> DeleteChangelog(string changelogId)
        {
            if (!await database.ChangelogRepository.DeleteByColumn(new("id", changelogId)))
                throw new DatabaseException();

            filesManager.DeleteDirectory($"files/changelogs/{changelogId}");

            return true;
        }

        #region private

        private async Task UploadImage(IFormFile image, Changelog changelog)
        {
            var uploadedPhoto = await filesManager.Upload(image, $"changelogs/{changelog.Id}");
            var changelogImage = ChangelogImage.Create(uploadedPhoto.Path, changelog.Id);

            await database.ChangelogImageRepository.Insert(changelogImage, false);

            changelog.SetImageUrl(uploadedPhoto.Path);
        }

        private async Task DeleteImage(Changelog changelog)
        {
            if (!await database.ChangelogImageRepository.DeleteByColumn(new("changelogId", changelog.Id)))
                throw new DatabaseException();

            filesManager.DeleteDirectory($"files/changelogs/{changelog.Id}");

            changelog.SetImageUrl(null);
        }

        #endregion
    }
}