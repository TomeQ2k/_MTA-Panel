using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MTA.Core.Application.Builders;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Services;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Shared.Services
{
    public class ReportImageService : IReportImageService
    {
        private readonly IDatabase database;
        private readonly IFilesManager filesManager;
        private readonly IReportValidationService reportValidationService;

        public ReportImageService(IDatabase database, IFilesManager filesManager,
            IReportValidationService reportValidationService)
        {
            this.database = database;
            this.filesManager = filesManager;
            this.reportValidationService = reportValidationService;
        }

        public async Task<bool> UploadReportImages(int userId, Report report, ICollection<IFormFile> images)
        {
            if (images == null || !images.Any())
                return false;

            if (!await reportValidationService.ValidateMaxFilesCountAndSizePerUser(userId, report, images))
            {
                await database.ReportRepository.Delete(report);
                throw new UploadFileException();
            }

            var uploadedPhotos = await filesManager.Upload(images, $"reports/{report.Id}");

            if (uploadedPhotos.Any(up => up == null))
            {
                filesManager.DeleteDirectory($"files/reports/{report.Id}");
                await database.ReportRepository.Delete(report);

                throw new UploadFileException();
            }

            var reportImages = uploadedPhotos.Select(up => new ReportImageBuilder()
                .SetLocation(up.Url, up.Path)
                .SetReportId(report.Id)
                .SetFileSize(up.Size)
                .SetUserId(userId)
                .Build());

            if (!await database.ReportImageRepository.InsertRange(reportImages, false))
                throw new DatabaseException();

            report.SetImages(reportImages);

            return true;
        }
    }
}