using System;

namespace MTA.Core.Application.Dtos
{
    public class GameItemDto
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public int OwnerId { get; set; }
        public int ItemId { get; set; }
        public string ItemValue { get; set; }
        public bool IsProtected { get; set; }
        public DateTime? LastUsed { get; set; }
        public string Imprint { get; set; }
    }
}