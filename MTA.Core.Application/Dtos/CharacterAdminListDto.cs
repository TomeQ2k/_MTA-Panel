using System;
using MTA.Core.Common.Enums;

namespace MTA.Core.Application.Dtos
{
    public class CharacterAdminListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AccountId { get; set; }
        public int Money { get; set; }
        public int BankMoney { get; set; }
        public GenderType Gender { get; set; }
        public string LastArea { get; set; }
        public int HoursPlayed { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Active { get; set; }
        public bool IsDead { get; set; }
        public int TotalMoney { get; set; }
    }
}