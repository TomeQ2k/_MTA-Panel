namespace MTA.Core.Application.Dtos
{
    public class UserAssigneeDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int AssignedReportsCount { get; set; }
    }
}