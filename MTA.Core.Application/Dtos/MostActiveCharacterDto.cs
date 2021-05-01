using System;

namespace MTA.Core.Application.Dtos
{
    public class MostActiveCharacterDto
    {
        public string Name { get; set; }
        public int HoursPlayed { get; set; }
        public DateTime LastLogin { get; set; }
    }
}