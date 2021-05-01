using System;
using System.Collections.Generic;
using System.Linq;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Domain.Entities
{
    public class User : EntityModel
    {
        [Column("id", true)] public int Id { get; protected set; }
        [Column("username")] public string Username { get; protected set; }
        [Column("email")] public string Email { get; protected set; }
        [Column("password")] public string PasswordHash { get; protected set; }
        [Column("salt")] public string PasswordSalt { get; protected set; }
        [Column("referrer")] public int ReferrerId { get; protected set; }
        [Column("mtaserial")] public string RegisterSerial { get; protected set; }
        [Column("activated")] public int Activated { get; protected set; }
        [Column("credits")] public int Credits { get; protected set; }
        [Column("lastlogin")] public DateTime LastLogin { get; protected set; }
        [Column("registerdate")] public DateTime RegisterDate { get; protected set; } = DateTime.Now;
        [Column("ip")] public string Ip { get; protected set; }

        [Column("adminjail")] public int AdminJail { get; protected set; }
        [Column("adminjail_time")] public int AdminJailTime { get; protected set; }
        [Column("adminjail_by")] public string AdminJailBy { get; protected set; }
        [Column("adminjail_reason")] public string AdminJailReason { get; protected set; }
        [Column("adminjail_permanent")] public int AdminJailPermanet { get; protected set; }

        [Column("warns")] public int Warns { get; protected set; }
        [Column("hours")] public int Hours { get; protected set; }
        [Column("serial_whitelist_cap")] public int SerialsLimit { get; protected set; }
        [Column("invitation_code")] public string InvitationCode { get; protected set; }
        [Column("appstate")] public int AppState { get; protected set; }
        [Column("adminreports")] public int AdminReportsCount { get; protected set; }
        [Column("admin")] public int AdminRole { get; protected set; }
        [Column("supporter")] public int SupporterRole { get; protected set; }
        [Column("vct")] public int VctRole { get; protected set; }
        [Column("mapper")] public int MapperRole { get; protected set; }
        [Column("scripter")] public int ScripterRole { get; protected set; }

        [Column("BanId", customProperty: true)]
        public int? BanId { get; protected set; }

        public Token Token { get; protected set; }

        public ICollection<Serial> Serials { get; protected set; } = new HashSet<Serial>();
        public ICollection<Character> Characters { get; protected set; } = new HashSet<Character>();
        public ICollection<Report> AssignedReports { get; protected set; } = new HashSet<Report>();

        public void SetUsername(string username) => Username = username;
        public void SetEmail(string email) => Email = email;

        public void SetPassword(string passwordHash, string passwordSalt) =>
            (PasswordHash, PasswordSalt) = (passwordHash, passwordSalt);

        public void SetSerial(string serial) => RegisterSerial = serial;
        public void SetReferrer(int referrerId) => ReferrerId = referrerId;
        public void ActivateAccount() => Activated = 1;
        public void SetToken(Token token) => Token = token;

        public void SetRoles(int admin = 0, int supporter = 0, int vct = 0, int mapper = 0, int scripter = 0)
            => (AdminRole, SupporterRole, VctRole, MapperRole, ScripterRole) =
                (admin, supporter, vct, mapper, scripter);

        public void AddCredits(int credits) => Credits += credits;

        public User SetBanId(int? banId)
        {
            BanId = banId;
            return this;
        }

        public void SetAppState(AppStateType appStateType) => AppState = (int) appStateType;

        public bool IsActivated => Activated == 1;

        public bool IsBlocked => BanId != null;

        public bool IsAppStateGreaterThan(AppStateType appStateType) => AppState > (int) appStateType;
        public bool IsAppStateLowerThan(AppStateType appStateType) => AppState < (int) appStateType;

        public bool HasEmptySerialSlot() => Serials.Count() < SerialsLimit;
        public void IncreaseSerialsLimit(int amount) => SerialsLimit += amount;
    }
}