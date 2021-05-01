using System;

namespace MTA.Core.Application.Dtos
{
    public class EstateDto
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public DateTime LastUsed { get; set; }
        public DateTime? ProtectedUntil { get; set; }
        public int InteriorId { get; set; }
    }
}