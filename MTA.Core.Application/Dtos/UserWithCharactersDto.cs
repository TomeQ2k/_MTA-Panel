using System;
using System.Collections.Generic;

namespace MTA.Core.Application.Dtos
{
    public class UserWithCharactersDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime LastLogin { get; set; }
        public string Ip { get; set; }
        public int AdminJail { get; set; }
        public int AdminJailTime { get; set; }
        public string AdminJailBy { get; set; }
        public string AdminJailReason { get; set; }
        public string AdminJailPermanent { get; set; }
        public int Warns { get; set; }
        public int Appstate { get; set; }
        public string Serial { get; set; }
        public int Credits { get; set; }
        public bool Activated { get; set; }
        public int SerialsLimit { get; set; }
        public int Hours { get; set; }
        public string InvitationCode { get; set; }
        public bool IsBlocked { get; set; }

        public ICollection<CharacterDto> Characters { get; set; }
        public ICollection<SerialDto> Serials { get; set; }
    }
}