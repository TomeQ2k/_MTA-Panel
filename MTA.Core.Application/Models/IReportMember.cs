using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Models
{
    public interface IReportMember
    {
        IReportMember DefineReportMember(Report report, User user);

        bool ValidateImagesForMember(int userId, IEnumerable<ReportImage> reportImages, IEnumerable<IFormFile> images);
    }
}