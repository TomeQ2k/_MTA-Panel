using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Helpers;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Models
{
    public class ReportSubscriberMember : IReportMember
    {
        public IReportMember DefineReportMember(Report report, User user)
            => report.ReportSubscribers.Any(rs => rs.UserId == user.Id && user.AdminRole == 0) ? this : null;

        public bool ValidateImagesForMember(int userId, IEnumerable<ReportImage> reportImages,
            IEnumerable<IFormFile> images)
        {
            int imagesCount = 0;
            long userReportImagesSize = 0;

            reportImages.Where(ri => ri.UserId == userId).ToList()
                .ForEach(ri => (imagesCount, userReportImagesSize) = (imagesCount + 1,
                    userReportImagesSize + ri.Size));

            foreach (var image in images)
            {
                if (userReportImagesSize + image.Length > Constants.MaximumReportImagesSizePerUser)
                    return false;

                if (image.Length <= Constants.MaximumReportImageSize)
                    imagesCount++;
                else if (image.Length <= Constants.MaximumReportImageSizePerRequest)
                    imagesCount += 2;
                else
                    return false;

                if (imagesCount > Constants.MaximumReportImagesCountPerUser)
                    throw new UploadFileException(
                        ValidatorMessages.MaxFilesCountValidatorMessage(Constants.MaximumReportImagesCountPerUser));
            }

            return true;
        }
    }
}