using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Helpers;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Models
{
    public class ReportAssigneeMember : IReportMember
    {
        public IReportMember DefineReportMember(Report report, User user)
            => report?.AssigneeId == user?.Id ? this : null;

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
                if (userReportImagesSize + image.Length <= Constants.MaximumReportImagesSizePerUser)
                    imagesCount += (int) Math.Ceiling(image.Length / (decimal) Constants.MaximumReportImageSize);
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