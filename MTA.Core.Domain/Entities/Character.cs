using System;
using System.Collections.Generic;
using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Domain.Entities
{
    public class Character : EntityModel
    {
        [Column("id", true)] public int Id { get; protected set; }
        [Column("charactername")] public string Name { get; protected set; }
        [Column("account")] public int AccountId { get; protected set; }
        [Column("money")] public int Money { get; protected set; }
        [Column("bankmoney")] public int BankMoney { get; protected set; }
        [Column("hoursplayed")] public int HoursPlayed { get; protected set; }
        [Column("lastlogin")] public DateTime LastLogin { get; protected set; }
        [Column("creationdate")] public DateTime CreationDate { get; protected set; }
        [Column("gender")] public int Gender { get; protected set; }
        [Column("skin")] public int SkinId { get; protected set; }
        [Column("lastarea")] public string LastArea { get; protected set; }
        [Column("age")] public int Age { get; protected set; }
        [Column("weight")] public int Weight { get; protected set; }
        [Column("height")] public int Height { get; protected set; }
        [Column("glod")] public int Hunger { get; protected set; }
        [Column("nawodnienie")] public int Irrigation { get; protected set; }
        [Column("kulturystyka")] public int Muscles { get; protected set; }
        [Column("job")] public int Job { get; protected set; }
        [Column("day")] public int Day { get; protected set; }
        [Column("month")] public int Month { get; protected set; }
        [Column("deaths")] public int Deaths { get; protected set; }
        [Column("death_date")] public DateTime DeathDate { get; protected set; }

        [Column("car_license")] public int CarLicense { get; protected set; }
        [Column("bike_license")] public int BikeLicense { get; protected set; }
        [Column("pilot_license")] public int PilotLicense { get; protected set; }
        [Column("fish_license")] public int FishLicense { get; protected set; }
        [Column("boat_license")] public int BoatLicense { get; protected set; }
        [Column("gun_license")] public int GunLicense { get; protected set; }
        [Column("gun2_license")] public int Gun2License { get; protected set; }
        [Column("active")] public int Active { get; protected set; }
        [Column("cked")] public int Dead { get; protected set; }

        [Column("faction_id")] public int FactionId { get; protected set; }
        [Column("faction_leader")] public int FactionLeader { get; protected set; }
        [Column("faction_rank")] public int FactionRank { get; protected set; }

        [Column("TotalMoney", customProperty: true)]
        public int TotalMoney { get; protected set; }

        public User User { get; protected set; }
        public Faction Faction { get; protected set; }

        public ICollection<Estate> Estates { get; protected set; } = new HashSet<Estate>();
        public ICollection<Vehicle> Vehicles { get; protected set; } = new HashSet<Vehicle>();

        public void SetUser(User user) => User = user;
        public void SetFaction(Faction faction) => Faction = faction;
        public void ToggleActive() => Active = Active == 1 ? 0 : 1;

        public void AddMoney(int money, int bankMoney)
            => (Money, BankMoney) = (Money + money, BankMoney + bankMoney);

        public bool IsActive => Active == 1;

        public bool IsDead => Dead == 1;

        public bool IsFactionLeader => FactionLeader == 1;
    }
}