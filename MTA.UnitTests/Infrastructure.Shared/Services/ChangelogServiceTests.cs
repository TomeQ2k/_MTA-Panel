using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using MTA.Core.Application.Builders;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Models;
using MTA.Core.Application.Services;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;
using MTA.Infrastructure.Shared.Services;
using NUnit.Framework;

namespace MTA.UnitTests.Infrastructure.Shared.Services
{
    [TestFixture]
    public class ChangelogServiceTests
    {
        private ChangelogService changelogService;

        private Mock<IDatabase> database;
        private Mock<IFilesManager> filesManager;
        private Mock<IMapper> mapper;

        private CreateChangelogRequest createRequest;
        private UpdateChangelogRequest updateRequest;

        private const string Id = "id";
        private const string ChangelogId = "changelogId";

        [SetUp]
        public void SetUp()
        {
            createRequest = new CreateChangelogRequest()
            {
                Title = "title",
                Content = "content",
                Image = It.IsAny<IFormFile>()
            };

            updateRequest = new UpdateChangelogRequest()
            {
                Title = "title",
                Content = "content",
                Image = It.IsAny<IFormFile>(),
                IsImageDeleted = false,
                ChangelogId = It.IsAny<string>()
            };

            database = new Mock<IDatabase>();
            filesManager = new Mock<IFilesManager>();
            mapper = new Mock<IMapper>();

            changelogService = new ChangelogService(database.Object, filesManager.Object, mapper.Object);
        }

        #region CreateChangelog

        [Test]
        public void CreateChangelog_InsertingToDatabaseFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.ChangelogRepository.Insert(It.IsNotNull<Changelog>(), false))
                .ReturnsAsync(false);

            Assert.That(() => changelogService.CreateChangelog(createRequest),
                Throws.Exception.TypeOf<DatabaseException>());
        }

        [Test]
        public async Task CreateChangelog_ImageUploadedAndInsertingSucceeded_ReturnChangelog()
        {
            createRequest = createRequest with {Image = It.IsNotNull<IFormFile>()};

            database.Setup(d => d.ChangelogRepository.Insert(It.IsNotNull<Changelog>(), false))
                .ReturnsAsync(true);
            database.Setup(d => d.ChangelogRepository.Update(It.IsNotNull<Changelog>()))
                .ReturnsAsync(true);

            var result = await changelogService.CreateChangelog(createRequest);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<Changelog>());
        }

        [Test]
        public async Task CreateChangelog_ImageNotUploadedAndInsertingSucceeded_ReturnChangelog()
        {
            database.Setup(d => d.ChangelogRepository.Insert(It.IsNotNull<Changelog>(), false))
                .ReturnsAsync(true);

            var result = await changelogService.CreateChangelog(createRequest);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<Changelog>());
        }

        #endregion

        #region UpdateChangelog

        [Test]
        public void UpdateChangelog_ChangelogNotFound_ThrowEntityNotFoundException()
        {
            database.Setup(d => d.ChangelogRepository.FindById(updateRequest.ChangelogId))
                .ReturnsAsync(() => null);

            Assert.That(() => changelogService.UpdateChangelog(updateRequest),
                Throws.Exception.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public async Task UpdateChangelog_ImageIsDeleted_DeleteImageShouldBeCalled()
        {
            updateRequest = updateRequest with {IsImageDeleted = true, Image = It.IsNotNull<IFormFile>()};

            var changelog = SetUpUpdateChangelogWhenImageIsDeleted();
            database.Setup(d => d.ChangelogImageRepository.DeleteByColumn(new(ChangelogId, changelog.Id))).ReturnsAsync(true);

            await changelogService.UpdateChangelog(updateRequest);

            database.Verify(d => d.ChangelogImageRepository.DeleteByColumn(new(ChangelogId, changelog.Id)));
            filesManager.Verify(fm => fm.DeleteDirectory(It.IsAny<string>(), true));
        }

        [Test]
        public void UpdateChangelog_DeletingImageFailed_ThrowDatabaseException()
        {
            updateRequest = updateRequest with {IsImageDeleted = true, Image = It.IsNotNull<IFormFile>()};

            var changelog = SetUpUpdateChangelogWhenImageIsDeleted();
            database.Setup(d => d.ChangelogImageRepository.DeleteByColumn(new(ChangelogId, changelog.Id)))
                .ReturnsAsync(false);

            Assert.That(() => changelogService.UpdateChangelog(updateRequest),
                Throws.Exception.TypeOf<DatabaseException>());
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task UpdateChangelog_ImageIsChanged_DeleteImageAndInsertImageShouldBeCalled(
            bool imageInserted)
        {
            updateRequest = updateRequest with
            {
                IsImageDeleted = false, Image = new FormFile(It.IsNotNull<Stream>(), It.IsNotNull<long>(),
                    It.IsNotNull<long>(),
                    It.IsNotNull<string>(), It.IsNotNull<string>())
            };

            var changelog = SetUpUpdateChangelogWhenImageIsChanged(imageInserted);
            database.Setup(d => d.ChangelogImageRepository.DeleteByColumn(new(ChangelogId, changelog.Id))).ReturnsAsync(true);

            await changelogService.UpdateChangelog(updateRequest);

            database.Verify(d => d.ChangelogImageRepository.DeleteByColumn(new(ChangelogId, changelog.Id)));
            filesManager.Verify(fm => fm.DeleteDirectory(It.IsAny<string>(), true));
            filesManager.Verify(fm => fm.Upload(It.IsNotNull<IFormFile>(), It.IsNotNull<string>()));
            database.Verify(d => d.ChangelogImageRepository.Insert(It.IsNotNull<ChangelogImage>(), false));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void UpdateChangelog_DuringChangingImageDeletingFailed_ThrowDatabaseException(bool imageInserted)
        {
            updateRequest = updateRequest with
            {
                IsImageDeleted = false, Image = new FormFile(It.IsNotNull<Stream>(), It.IsNotNull<long>(),
                    It.IsNotNull<long>(),
                    It.IsNotNull<string>(), It.IsNotNull<string>())
            };

            var changelog = SetUpUpdateChangelogWhenImageIsChanged(imageInserted);
            database.Setup(d => d.ChangelogImageRepository.DeleteByColumn(new(ChangelogId, changelog.Id)))
                .ReturnsAsync(false);

            Assert.That(() => changelogService.UpdateChangelog(updateRequest),
                Throws.Exception.TypeOf<DatabaseException>());
        }

        [Test]
        public async Task UpdateChangelog_ImageIsDeleted_ReturnChangelog()
        {
            updateRequest = updateRequest with {IsImageDeleted = true, Image = It.IsNotNull<IFormFile>()};

            var changelog = SetUpUpdateChangelogWhenImageIsDeleted();
            database.Setup(d => d.ChangelogImageRepository.DeleteByColumn(new(ChangelogId, changelog.Id))).ReturnsAsync(true);

            var result = await changelogService.UpdateChangelog(updateRequest);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<Changelog>());
            Assert.That(result.Id, Is.EqualTo(changelog.Id));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task UpdateChangelog_ImageIsChanged_ReturnChangelog(bool imageInserted)
        {
            updateRequest = updateRequest with
            {
                IsImageDeleted = false, Image = new FormFile(It.IsNotNull<Stream>(), It.IsNotNull<long>(),
                    It.IsNotNull<long>(),
                    It.IsNotNull<string>(), It.IsNotNull<string>())
            };

            var changelog = SetUpUpdateChangelogWhenImageIsChanged(imageInserted);
            database.Setup(d => d.ChangelogImageRepository.DeleteByColumn(new(ChangelogId, changelog.Id))).ReturnsAsync(true);

            var result = await changelogService.UpdateChangelog(updateRequest);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<Changelog>());
            Assert.That(result.Id, Is.EqualTo(changelog.Id));
        }

        #endregion

        #region DeleteChangelog

        [Test]
        public void DeleteChangelog_DeletingFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.ChangelogRepository.DeleteByColumn(new(Id, It.IsNotNull<string>()))).ReturnsAsync(false);

            Assert.That(() => changelogService.DeleteChangelog(It.IsNotNull<string>()),
                Throws.Exception.TypeOf<DatabaseException>());
        }

        [Test]
        public async Task DeleteChangelog_ChangelogIsDeleted_ImageShouldBeDeleted()
        {
            database.Setup(d => d.ChangelogRepository.DeleteByColumn(new(Id, It.IsNotNull<string>()))).ReturnsAsync(true);

            await changelogService.DeleteChangelog(It.IsNotNull<string>());

            filesManager.Verify(fm => fm.DeleteDirectory(It.IsNotNull<string>(), true));
        }

        [Test]
        public async Task DeleteChangelog_ChangelogIsDeleted_ReturnTrue()
        {
            database.Setup(d => d.ChangelogRepository.DeleteByColumn(new(Id, It.IsNotNull<string>()))).ReturnsAsync(true);

            var result = await changelogService.DeleteChangelog(It.IsNotNull<string>());

            Assert.That(result, Is.True);
        }

        #endregion

        #region private

        private Changelog SetUpUpdateChangelogWhenImageIsDeleted()
        {
            var changelog = new ChangelogBuilder()
                .SetTitle(It.IsNotNull<string>())
                .SetContent(It.IsNotNull<string>())
                .SetImageUrl("url")
                .Build();

            database.Setup(d => d.ChangelogRepository.FindById(updateRequest.ChangelogId))
                .ReturnsAsync(changelog);
            mapper.Setup(m => m.Map(updateRequest, changelog)).Returns(changelog);
            database.Setup(d => d.ChangelogRepository.Update(changelog)).ReturnsAsync(true);

            return changelog;
        }

        private Changelog SetUpUpdateChangelogWhenImageIsChanged(bool imageInserted)
        {
            var changelog = new ChangelogBuilder()
                .SetTitle(It.IsNotNull<string>())
                .SetContent(It.IsNotNull<string>())
                .SetImageUrl(It.IsAny<string>())
                .Build();

            database.Setup(d => d.ChangelogRepository.FindById(updateRequest.ChangelogId))
                .ReturnsAsync(changelog);
            mapper.Setup(m => m.Map(updateRequest, changelog)).Returns(changelog);
            filesManager.Setup(fm => fm.Upload(It.IsNotNull<IFormFile>(), It.IsNotNull<string>()))
                .ReturnsAsync(new FileModel("path", "url", 1));
            database.Setup(d => d.ChangelogImageRepository.Insert(It.IsNotNull<ChangelogImage>(), false))
                .ReturnsAsync(imageInserted);
            database.Setup(d => d.ChangelogRepository.Update(changelog)).ReturnsAsync(true);

            return changelog;
        }

        #endregion
    }
}