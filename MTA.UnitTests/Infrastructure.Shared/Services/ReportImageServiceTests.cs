using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Models;
using MTA.Core.Application.Services;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;
using MTA.Infrastructure.Shared.Services;
using NUnit.Framework;

namespace MTA.UnitTests.Infrastructure.Shared.Services
{
    [TestFixture]
    public class ReportImageServiceTests
    {
        private Report report;

        private Mock<IDatabase> database;
        private Mock<IFilesManager> filesManager;
        private Mock<IReportValidationService> reportValidationService;

        private ReportImageService reportImageService;
        private List<IFormFile> images;
        private List<FileModel> photos;

        private const int UserId = 1;

        [SetUp]
        public void SetUp()
        {
            report = new Report();
            var image = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data",
                "dummy.txt");

            images = new List<IFormFile>
            {
                image,
                image,
            };

            photos = new List<FileModel>
            {
                new("1", "1", 1),
                new("1", "1", 2),
            };

            database = new Mock<IDatabase>();
            filesManager = new Mock<IFilesManager>();
            reportValidationService = new Mock<IReportValidationService>();

            reportValidationService.Setup(rv =>
                    rv.ValidateMaxFilesCountAndSizePerUser(It.IsAny<int>(), It.IsAny<Report>(),
                        It.IsAny<IEnumerable<IFormFile>>()))
                .ReturnsAsync(true);
            filesManager.Setup(fm => fm.Upload(It.IsAny<IEnumerable<IFormFile>>(), It.IsAny<string>()))
                .ReturnsAsync(photos);
            database.Setup(d => d.ReportImageRepository.InsertRange(It.IsAny<IEnumerable<ReportImage>>(), false))
                .ReturnsAsync(true);
            database.Setup(d => d.ReportRepository.Delete(It.IsAny<Report>()));

            reportImageService =
                new ReportImageService(database.Object, filesManager.Object, reportValidationService.Object);
        }

        #region UploadReportImages

        [Test]
        public async Task UploadReportImages_NullImages_ReturnFalse()
        {
            images = null;
            var result = await reportImageService.UploadReportImages(UserId, report, images);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task UploadReportImages_EmptyImages_ReturnFalse()
        {
            images = new List<IFormFile>();
            var result = await reportImageService.UploadReportImages(UserId, report, images);

            Assert.That(result, Is.False);
        }

        [Test]
        public void UploadReportImages_ValidateMaxFilesCountAndSizePerUserFailed_ThrowUploadFileException()
        {
            reportValidationService.Setup(rv =>
                    rv.ValidateMaxFilesCountAndSizePerUser(It.IsAny<int>(), It.IsAny<Report>(),
                        It.IsAny<IEnumerable<IFormFile>>()))
                .ReturnsAsync(false);

            Assert.That(() => reportImageService.UploadReportImages(UserId, report, images),
                Throws.TypeOf<UploadFileException>());
        }

        [Test]
        public void UploadReportImages_ImageFilesUploadReturnInvalidList_ThrowUploadFileException()
        {
            filesManager.Setup(fm => fm.Upload(It.IsAny<IEnumerable<IFormFile>>(), It.IsAny<string>()))
                .ReturnsAsync(new List<FileModel>
                {
                    null
                });

            Assert.That(() => reportImageService.UploadReportImages(UserId, report, images),
                Throws.TypeOf<UploadFileException>());
        }

        [Test]
        public void UploadReportImages_InsertReportImagesFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.ReportImageRepository.InsertRange(It.IsAny<IEnumerable<ReportImage>>(), false))
                .ReturnsAsync(false);                
            Assert.That(() => reportImageService.UploadReportImages(UserId, report, images),
                Throws.TypeOf<DatabaseException>());
        }

        [Test]
        public async Task UploadReportImages_WhenCalled_ReturnTrue()
        {
            var result = await reportImageService.UploadReportImages(UserId, report, images);

            Assert.That(result, Is.True);
        }

        #endregion
    }
}