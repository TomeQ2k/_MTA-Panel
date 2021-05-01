using MTA.Core.Common.Enums;

namespace MTA.Core.Application.Dtos
{
    public class BugReportDto
    {
        public string Id { get; set; }
        public string ReportId { get; set; }
        public ReportBugType BugType { get; set; }
        public string AdditionalInfo { get; set; }
        
        public ReportDto Report { get; set; }
    }
}