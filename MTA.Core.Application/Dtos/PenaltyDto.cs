using System;

namespace MTA.Core.Application.Dtos
{
    public class PenaltyDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Reason { get; set; }
        public int AdminId { get; set; }
        public int AccountId { get; set; }
        public int Duration { get; set; }
    }
}