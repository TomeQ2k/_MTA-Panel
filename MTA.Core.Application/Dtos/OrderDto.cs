using System;
using MTA.Core.Common.Enums;

namespace MTA.Core.Application.Dtos
{
    public class OrderDto
    {
        public string Id { get; set; }
        public PremiumOperation Operation { get; set; }
        public int Cost { get; set; }
        public int? UserId { get; set; }
        public int? CharacterId { get; set; }
        public int? EstateId { get; set; }
        public StateType ApprovalState { get; set; }
        public string AdminNote { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateReviewed { get; set; }

        public PremiumFileDto PremiumFile { get; set; }
    }
}