using MTA.Core.Domain.Data.Repositories;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Domain.Data
{
    public interface IDatabase : IDatabaseExecutor
    {
        #region repositories

        IUserRepository UserRepository { get; }
        ICharacterRepository CharacterRepository { get; }
        IFactionRepository FactionRepository { get; }
        IRepository<Estate> EstateRepository { get; }
        IRepository<Vehicle> VehicleRepository { get; }
        IRepository<VehiclesShop> VehiclesShopRepository { get; }
        IArticleRepository ArticleRepository { get; }
        IRepository<ArticleImage> ArticleImageRepository { get; }
        IRepository<Changelog> ChangelogRepository { get; }
        IRepository<ChangelogImage> ChangelogImageRepository { get; }
        ITokenRepository TokenRepository { get; }
        ISerialRepository SerialRepository { get; }
        IRepository<Ban> BanRepository { get; }
        IConnectionRepository ConnectionRepository { get; }
        INotificationRepository NotificationRepository { get; }
        IRepository<Question> QuestionRepository { get; }
        IApplicationRepository ApplicationRepository { get; }
        IReportRepository ReportRepository { get; }
        IRepository<PenaltyReport> PenaltyReportRepository { get; }
        IRepository<UserReport> UserReportRepository { get; }
        IRepository<BugReport> BugReportRepository { get; }
        IRepository<ReportSubscriber> ReportSubscriberRepository { get; }
        IRepository<ReportComment> ReportCommentRepository { get; }
        IRepository<ReportImage> ReportImageRepository { get; }
        IAdminActionRepository AdminActionRepository { get; }
        IMtaLogRepository MtaLogRepository { get; }
        IPhoneSmsRepository PhoneSmsRepository { get; }
        IPurchaseRepository PurchaseRepository { get; }
        IOrderRepository OrderRepository { get; }
        IPremiumFileRepository PremiumFileRepository { get; }
        IGameItemRepository GameItemRepository { get; }
        IRepository<GameTempObject> GameTempObjectRepository { get; }
        IRepository<GameTempInterior> GameTempInteriorRepository { get; }
        IRepository<OrderTransaction> OrderTransactionRepository { get; }

        #endregion
    }
}