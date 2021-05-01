using System;
using System.Collections.Generic;
using MTA.Core.Common.Enums;

namespace MTA.Core.Application.Dtos
{
    public class CharacterDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SkinId { get; set; }
        public int Money { get; set; }
        public int BankMoney { get; set; }
        public GenderType Gender { get; set; }
        public bool IsDead { get; set; }
        public string LastArea { get; set; }
        public int Age { get; set; }
        public int Weight { get; set; }
        public int Height { get; set; }
        public int Deaths { get; set; }
        public int CarLicense { get; set; }
        public int BikeLicense { get; set; }
        public int PilotLicense { get; set; }
        public int FishLicense { get; set; }
        public int BoatLicense { get; set; }
        public int GunLicense { get; set; }
        public int Gun2License { get; set; }
        public int HoursPlayed { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Active { get; set; }
        public int Job { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Hunger { get; set; }
        public int Irrigation { get; set; }
        public int Muscles { get; set; }
        public int FactionId { get; set; }
        public bool IsFactionLeader { get; set; }
        public int FactionRank { get; set; }
        public string FactionRankName { get; set; }

        public DateTime DeathDate { get; set; }
        public FactionDto Faction { get; set; }

        public ICollection<VehicleDto> Vehicles { get; set; }
        public ICollection<EstateDto> Estates { get; set; }
    }
}