using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Domain.Entities
{
    public class PremiumFile : BaseFile
    {
        [Column("orderId")] public string OrderId { get; protected set; }
        [Column("userId")] public int UserId { get; protected set; }
        [Column("fileType")] public int FileType { get; protected set; }
        [Column("skinId")] public int? SkinId { get; protected set; }

        public Order Order { get; protected set; }
        public Estate Estate { get; protected set; }

        public PremiumFile SetOrderId(string orderId)
        {
            OrderId = orderId;
            return this;
        }

        public PremiumFile SetSkin(int? skinId)
        {
            SkinId = skinId;
            return this;
        }

        public PremiumFile SetFileType(PremiumFileType fileType)
        {
            FileType = (int) fileType;
            return this;
        }

        public PremiumFile SetUserId(int userId)
        {
            UserId = userId;
            return this;
        }

        public void SetOrder(Order order)
        {
            Order = order;
        }

        public void SetEstate(Estate estate)
        {
            Estate = estate;
        }
    }
}