namespace MTA.Core.Application.Dtos
{
    public class UserReportDto
    {
        public string Id { get; set; }
        public string ReportId { get; set; }
        public int UserId { get; set; }
        public int WitnessId { get; set; }
        
        public ReportDto Report { get; set; }
    }
}