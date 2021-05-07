using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Domain.Entities
{
    public class ReportImage : BaseFile
    {
        [Column("reportId")] public string ReportId { get; protected set; }
        [Column("userId")] public int UserId { get; protected set; }
        [Column("size")] public long Size { get; protected set; }

        public void SetLocation(string path) => Path = path;
        public void SetReportId(string reportId) => ReportId = reportId;
        public void SetUserId(int userId) => UserId = userId;
        public void SetFileSize(long size) => Size = size;
    }
}