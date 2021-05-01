using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Services
{
    public interface IReportImageService
    {
        Task<bool> UploadReportImages(int userId, Report report, ICollection<IFormFile> images);
    }
}