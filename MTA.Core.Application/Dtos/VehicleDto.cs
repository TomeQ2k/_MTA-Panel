using System;

namespace MTA.Core.Application.Dtos
{
    public class VehicleDto
    {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public string Plate { get; set; }
        public int OwnerId { get; set; }
        public DateTime LastUsed { get; set; }
        public DateTime? ProtectedUntil { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
    }
}