using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MTA.Core.Application.Helpers;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Data.Helpers;
using MTA.Core.Domain.Data.Repositories;
using MTA.Core.Domain.Entities;
using MTA.Infrastructure.Persistence.Database.Repositories;

#pragma warning disable 649
#pragma warning disable 169

namespace MTA.Infrastructure.Persistence.Database
{
    public class Database : IDatabase
    {
        protected readonly ISqlConnectionFactory connectionFactory;

        public Database(ISqlConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task<bool> Execute(SqlQuery query)
        {
            using (var connection = connectionFactory.Connection)
            {
                return await connection.ExecuteAsync(query.Query, commandTimeout: SqlConstants.CommandTimeout) > 0;
            }
        }

        public async Task<IEnumerable<TValue>> SelectQuery<TValue>(SqlQuery query)
        {
            using (var connection = connectionFactory.Connection)
            {
                return await connection.QueryAsync<TValue>(query.Query, commandTimeout: SqlConstants
                    .CommandTimeout);
            }
        }

        public async Task<TValue> SelectQueryFirst<TValue>(SqlQuery query)
        {
            using (var connection = connectionFactory.Connection)
            {
                return (await connection.QueryAsync<TValue>(query.Query, commandTimeout: SqlConstants.CommandTimeout))
                    .FirstOrDefault();
            }
        }

        public IDatabaseTransaction BeginTransaction() => new DatabaseTransaction();

        #region repositories

        private IUserRepository userRepository;

        public IUserRepository UserRepository => userRepository ?? new UserRepository(connectionFactory,
            RepositoryDictionary.FindTable(typeof(IUserRepository)));

        private ICharacterRepository characterRepository;

        public ICharacterRepository CharacterRepository => characterRepository ?? new CharacterRepository(
            connectionFactory,
            RepositoryDictionary.FindTable(typeof(ICharacterRepository)));

        private IFactionRepository factionRepository;

        public IFactionRepository FactionRepository => factionRepository ?? new FactionRepository(
            connectionFactory,
            RepositoryDictionary.FindTable(typeof(IFactionRepository)));

        private IRepository<Estate> estateRepository;

        public IRepository<Estate> EstateRepository => estateRepository ?? new Repository<Estate>(
            connectionFactory,
            RepositoryDictionary.FindTable(typeof(IRepository<Estate>)));

        private IRepository<Vehicle> vehicleRepository;

        public IRepository<Vehicle> VehicleRepository => vehicleRepository ?? new Repository<Vehicle>(
            connectionFactory,
            RepositoryDictionary.FindTable(typeof(IRepository<Vehicle>)));

        private IRepository<VehiclesShop> vehiclesShopRepository;

        public IRepository<VehiclesShop> VehiclesShopRepository => vehiclesShopRepository ??
                                                                   new Repository<VehiclesShop>(
                                                                       connectionFactory,
                                                                       RepositoryDictionary.FindTable(
                                                                           typeof(IRepository<VehiclesShop>)));

        private IArticleRepository articleRepository;

        public IArticleRepository ArticleRepository => articleRepository ?? new ArticleRepository(
            connectionFactory,
            RepositoryDictionary.FindTable(typeof(IArticleRepository)));

        private IRepository<ArticleImage> articlePhotoRepository;

        public IRepository<ArticleImage> ArticleImageRepository => articlePhotoRepository ??
                                                                   new Repository<ArticleImage>(
                                                                       connectionFactory,
                                                                       RepositoryDictionary.FindTable(
                                                                           typeof(IRepository<ArticleImage>)));

        private IRepository<Changelog> changelogRepository;

        public IRepository<Changelog> ChangelogRepository => changelogRepository ?? new Repository<Changelog>(
            connectionFactory,
            RepositoryDictionary.FindTable(typeof(IRepository<Changelog>)));

        private IRepository<ChangelogImage> changelogPhotoRepository;

        public IRepository<ChangelogImage> ChangelogImageRepository => changelogPhotoRepository ??
                                                                       new Repository<ChangelogImage>(
                                                                           connectionFactory,
                                                                           RepositoryDictionary.FindTable(
                                                                               typeof(IRepository<ChangelogImage>)));

        private ITokenRepository tokenRepository;

        public ITokenRepository TokenRepository => tokenRepository ?? new TokenRepository(connectionFactory,
            RepositoryDictionary.FindTable(typeof(ITokenRepository)));

        private ISerialRepository serialRepository;

        public ISerialRepository SerialRepository => serialRepository ?? new SerialRepository(connectionFactory,
            RepositoryDictionary.FindTable(typeof(ISerialRepository)));

        private IRepository<Ban> banRepository;

        public IRepository<Ban> BanRepository => banRepository ?? new Repository<Ban>(connectionFactory,
            RepositoryDictionary.FindTable(typeof(IRepository<Ban>)));

        private IConnectionRepository connectionRepository;

        public IConnectionRepository ConnectionRepository => connectionRepository ?? new ConnectionRepository(
            connectionFactory,
            RepositoryDictionary.FindTable(typeof(IConnectionRepository)));

        private INotificationRepository notificationRepository;

        public INotificationRepository NotificationRepository => notificationRepository ?? new NotificationRepository(
            connectionFactory,
            RepositoryDictionary.FindTable(typeof(INotificationRepository)));

        private IRepository<Question> questionRepository;

        public IRepository<Question> QuestionRepository => questionRepository ?? new Repository<Question>(
            connectionFactory,
            RepositoryDictionary.FindTable(typeof(IRepository<Question>)));

        private IApplicationRepository applicationRepository;

        public IApplicationRepository ApplicationRepository => applicationRepository ?? new ApplicationRepository(
            connectionFactory,
            RepositoryDictionary.FindTable(typeof(IApplicationRepository)));

        private IReportRepository reportRepository;

        public IReportRepository ReportRepository => reportRepository ?? new ReportRepository(
            connectionFactory,
            RepositoryDictionary.FindTable(typeof(IReportRepository)));

        private IRepository<PenaltyReport> penaltyRepository;

        public IRepository<PenaltyReport> PenaltyReportRepository => penaltyRepository ?? new Repository<PenaltyReport>(
            connectionFactory,
            RepositoryDictionary.FindTable(typeof(IRepository<PenaltyReport>)));

        private IRepository<UserReport> userReportRepository;

        public IRepository<UserReport> UserReportRepository => userReportRepository ?? new Repository<UserReport>(
            connectionFactory,
            RepositoryDictionary.FindTable(typeof(IRepository<UserReport>)));

        private IRepository<BugReport> reportBugRepository;

        public IRepository<BugReport> BugReportRepository => reportBugRepository ?? new Repository<BugReport>(
            connectionFactory,
            RepositoryDictionary.FindTable(typeof(IRepository<BugReport>)));

        private IRepository<ReportSubscriber> reportSubscriberRepository;

        public IRepository<ReportSubscriber> ReportSubscriberRepository => reportSubscriberRepository ??
                                                                           new Repository<ReportSubscriber>(
                                                                               connectionFactory,
                                                                               RepositoryDictionary.FindTable(
                                                                                   typeof(IRepository<ReportSubscriber>
                                                                                   )));

        private IRepository<ReportComment> reportCommentRepository;

        public IRepository<ReportComment> ReportCommentRepository => reportCommentRepository ??
                                                                     new Repository<ReportComment>(connectionFactory,
                                                                         RepositoryDictionary.FindTable(
                                                                             typeof(IRepository<ReportComment>
                                                                             )));

        private IRepository<ReportImage> reportImageRepository;

        public IRepository<ReportImage> ReportImageRepository => reportImageRepository ??
                                                                 new Repository<ReportImage>(connectionFactory,
                                                                     RepositoryDictionary.FindTable(
                                                                         typeof(IRepository<ReportImage>
                                                                         )));

        private IAdminActionRepository adminActionRepository;

        public IAdminActionRepository AdminActionRepository => adminActionRepository ??
                                                               new AdminActionRepository(connectionFactory,
                                                                   RepositoryDictionary.FindTable(
                                                                       typeof(IAdminActionRepository)));

        private IMtaLogRepository mtaLogRepository;

        public IMtaLogRepository MtaLogRepository => mtaLogRepository ??
                                                     new MtaLogRepository(connectionFactory,
                                                         RepositoryDictionary.FindTable(typeof(IMtaLogRepository)));

        private IPhoneSmsRepository phoneSmsRepository;

        public IPhoneSmsRepository PhoneSmsRepository => phoneSmsRepository ??
                                                         new PhoneSmsRepository(connectionFactory,
                                                             RepositoryDictionary.FindTable(
                                                                 typeof(IPhoneSmsRepository)));

        private IPurchaseRepository purchaseRepository;

        public IPurchaseRepository PurchaseRepository => purchaseRepository ??
                                                         new PurchaseRepository(connectionFactory,
                                                             RepositoryDictionary.FindTable(
                                                                 typeof(IPurchaseRepository)));

        private IOrderRepository orderRepository;

        public IOrderRepository OrderRepository => orderRepository ??
                                                   new OrderRepository(connectionFactory,
                                                       RepositoryDictionary.FindTable(
                                                           typeof(IOrderRepository)));

        private IPremiumFileRepository premiumFileRepository;

        public IPremiumFileRepository PremiumFileRepository => premiumFileRepository ??
                                                               new PremiumFileRepository(connectionFactory,
                                                                   RepositoryDictionary.FindTable(
                                                                       typeof(IPremiumFileRepository)));

        private IGameItemRepository gameItemRepository;

        public IGameItemRepository GameItemRepository => gameItemRepository ??
                                                         new GameItemRepository(connectionFactory,
                                                             RepositoryDictionary.FindTable(
                                                                 typeof(IGameItemRepository)));

        private IRepository<GameTempObject> gameTempObjectRepository;

        public IRepository<GameTempObject> GameTempObjectRepository => gameTempObjectRepository ??
                                                                       new Repository<GameTempObject>(connectionFactory,
                                                                           RepositoryDictionary.FindTable(
                                                                               typeof(IRepository<GameTempObject>)));

        private IRepository<GameTempInterior> gameTempInteriorRepository;

        public IRepository<GameTempInterior> GameTempInteriorRepository => gameTempInteriorRepository ??
                                                                           new Repository<GameTempInterior>(
                                                                               connectionFactory,
                                                                               RepositoryDictionary.FindTable(
                                                                                   typeof(IRepository<GameTempInterior>
                                                                                   )));

        private IRepository<OrderTransaction> orderTransactionRepository;

        public IRepository<OrderTransaction> OrderTransactionRepository => orderTransactionRepository ??
                                                                           new Repository<OrderTransaction>(
                                                                               connectionFactory,
                                                                               RepositoryDictionary.FindTable(
                                                                                   typeof(IRepository<OrderTransaction>
                                                                                   )));

        #endregion
    }
}