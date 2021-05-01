using System;
using MTA.Core.Common.Enums;

namespace MTA.Core.Application.Dtos
{
    public class AdminActionDto
    {
        public int Id { get; set; }
        public string Reason { get; set; }
        public int UserId { get; set; }
        public int CharacterId { get; set; }
        public int AdminId { get; set; }
        public DateTime DateAdded { get; set; }
        public AdminActionType Action { get; set; }
        public int Duration { get; set; }
        public string UserName { get; set; }
        public string AdminName { get; set; }
        public string CharacterName { get; set; }
    }
}