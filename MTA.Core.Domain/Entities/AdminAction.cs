using System;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Domain.Entities
{
    public class AdminAction : EntityModel
    {
        [Column("id", true)] public int Id { get; protected set; }
        [Column("reason")] public string Reason { get; protected set; }
        [Column("user")] public int UserId { get; protected set; }
        [Column("user_char")] public int CharacterId { get; protected set; }
        [Column("admin")] public int AdminId { get; protected set; }
        [Column("date")] public DateTime DateAdded { get; protected set; } = DateTime.Now;
        [Column("action")] public int Action { get; protected set; } = (int) AdminActionType.Other;
        [Column("duration")] public int Duration { get; protected set; }

        [Column("UserName", customProperty: true)]
        public string UserName { get; protected set; }

        [Column("AdminName", customProperty: true)]
        public string AdminName { get; protected set; }

        [Column("CharacterName", customProperty: true)]
        public string CharacterName { get; protected set; }

        public void SetReason(string reason) => Reason = reason;

        public void SetUserId(int userId) => UserId = userId;

        public void SetCharacterId(int characterId) => CharacterId = characterId;

        public void SetAdminId(int adminId) => AdminId = adminId;

        public void SetAction(AdminActionType actionType) => Action = (int) actionType;

        public void SetDuration(int duration) => Duration = duration;
    }
}