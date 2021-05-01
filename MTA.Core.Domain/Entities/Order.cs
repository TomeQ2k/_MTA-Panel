using System;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Domain.Entities
{
    public class Order : EntityModel
    {
        [Column("id", true)] public string Id { get; protected set; } = Utils.Id();
        [Column("operation")] public int Operation { get; protected set; } = (int) PremiumOperation.Other;
        [Column("cost")] public int Cost { get; protected set; }
        [Column("userId")] public int? UserId { get; protected set; }
        [Column("characterId")] public int? CharacterId { get; protected set; }
        [Column("estateId")] public int? EstateId { get; protected set; }
        [Column("approvalState")] public int ApprovalState { get; protected set; } = (int) StateType.None;
        [Column("adminNote")] public string AdminNote { get; protected set; }
        [Column("dateCreated")] public DateTime DateCreated { get; protected set; } = DateTime.Now;
        [Column("dateReviewed")] public DateTime? DateReviewed { get; protected set; }

        public PremiumFile PremiumFile { get; protected set; }
        public User User { get; protected set; }

        public void SetOperation(PremiumOperation operation) => Operation = (int) operation;
        public void SetCost(int cost) => Cost = cost;
        public void SetUserId(int userId) => UserId = userId;
        public void SetEstateId(int estateId) => EstateId = estateId;
        public void SetCharacterId(int characterId) => CharacterId = characterId;
        public void SetApprovalState(StateType stateType) => ApprovalState = (int) stateType;
        public void SetAdminNote(string adminNote) => AdminNote = adminNote;
        public void Review() => DateReviewed = DateTime.Now;
        public void SetOrderFile(PremiumFile premiumFile) => PremiumFile = premiumFile;
        public void SetUser(User user) => User = user;

        public bool IsOrderReviewed => ApprovalState != (int) StateType.None;
    }
}