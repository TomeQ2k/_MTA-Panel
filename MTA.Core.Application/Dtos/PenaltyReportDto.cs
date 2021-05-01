namespace MTA.Core.Application.Dtos
{
    public class PenaltyReportDto
    {
        public string Id { get; set; }
        public string ReportId { get; set; }
        public int BanId { get; set; }
        public int PenaltyId { get; set; }
        
        public ReportDto Report { get; set; }
    }
}