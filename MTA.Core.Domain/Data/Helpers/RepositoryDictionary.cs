using System;
using System.Collections.Generic;
using MTA.Core.Domain.Data.Repositories;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Domain.Data.Helpers
{
    public static class RepositoryDictionary
    {
        #region repositories

        private static readonly (Type, string) userRepository = (typeof(IUserRepository), "konta");
        private static readonly (Type, string) characterRepository = (typeof(ICharacterRepository), "postacie");
        private static readonly (Type, string) factionRepository = (typeof(IFactionRepository), "factions");
        private static readonly (Type, string) estateRepository = (typeof(IRepository<Estate>), "nieruchomosci");
        private static readonly (Type, string) vehicleRepository = (typeof(IRepository<Vehicle>), "pojazdy");

        private static readonly (Type, string) vehiclesShopRepository =
            (typeof(IRepository<VehiclesShop>), "pojazdy_shop");

        private static readonly (Type, string) articleRepository = (typeof(IArticleRepository), "articles");

        private static readonly (Type, string) articleImageRepository =
            (typeof(IRepository<ArticleImage>), "article_images");

        private static readonly (Type, string) changelogRepository = (typeof(IRepository<Changelog>), "changelogs");

        private static readonly (Type, string) changelogImageRepository =
            (typeof(IRepository<ChangelogImage>), "changelog_images");

        private static readonly (Type, string) tokenRepository = (typeof(ITokenRepository), "tokens");
        private static readonly (Type, string) serialRepository = (typeof(ISerialRepository), "serial_whitelist");
        private static readonly (Type, string) banRepository = (typeof(IRepository<Ban>), "bans");
        private static readonly (Type, string) connectionRepository = (typeof(IConnectionRepository), "connections");

        private static readonly (Type, string) notificationRepository =
            (typeof(INotificationRepository), "notifications");

        private static readonly (Type, string) questionRepository =
            (typeof(IRepository<Question>), "aplikacje_pytania");

        private static readonly (Type, string) applicationRepository = (typeof(IApplicationRepository), "aplikacje");
        private static readonly (Type, string) reportRepository = (typeof(IReportRepository), "reports");

        private static readonly (Type, string) penaltyReportRepository =
            (typeof(IRepository<PenaltyReport>), "penalty_reports");

        private static readonly (Type, string) userReportRepository = (typeof(IRepository<UserReport>), "user_reports");
        private static readonly (Type, string) bugRepository = (typeof(IRepository<BugReport>), "bug_reports");

        private static readonly (Type, string) reportSubscriberRepository =
            (typeof(IRepository<ReportSubscriber>), "report_subscribers");

        private static readonly (Type, string) reportCommentRepository =
            (typeof(IRepository<ReportComment>), "report_comments");

        private static readonly (Type, string) reportImageRepository =
            (typeof(IRepository<ReportImage>), "report_images");

        private static readonly (Type, string) adminActionRepository =
            (typeof(IAdminActionRepository), "adminhistory");

        private static readonly (Type, string) mtaLogRepository =
            (typeof(IMtaLogRepository), "mta_logi");

        private static readonly (Type, string) phoneSmsRepository =
            (typeof(IPhoneSmsRepository), "phone_sms");

        private static readonly (Type, string) purchaseRepository =
            (typeof(IPurchaseRepository), "don_purchases");

        private static readonly (Type, string) orderRepository =
            (typeof(IOrderRepository), "orders");

        private static readonly (Type, string) premiumFileRepository =
            (typeof(IPremiumFileRepository), "premium_files");

        private static readonly (Type, string) gameItemRepository =
            (typeof(IGameItemRepository), "items");

        private static readonly (Type, string) gameTempObjectRepository =
            (typeof(IRepository<GameTempObject>), "tempobjects");

        private static readonly (Type, string) gameTempInteriorRepository =
            (typeof(IRepository<GameTempInterior>), "tempinteriors");

        private static readonly (Type, string) orderTransactionRepository =
            (typeof(IRepository<OrderTransaction>), "don_transactions");

        #endregion

        private static Dictionary<Type, string> RepositoriesDictionary =>
            new()
            {
                {userRepository.Item1, userRepository.Item2},
                {characterRepository.Item1, characterRepository.Item2},
                {factionRepository.Item1, factionRepository.Item2},
                {estateRepository.Item1, estateRepository.Item2},
                {vehicleRepository.Item1, vehicleRepository.Item2},
                {vehiclesShopRepository.Item1, vehiclesShopRepository.Item2},
                {articleRepository.Item1, articleRepository.Item2},
                {articleImageRepository.Item1, articleImageRepository.Item2},
                {changelogRepository.Item1, changelogRepository.Item2},
                {changelogImageRepository.Item1, changelogImageRepository.Item2},
                {tokenRepository.Item1, tokenRepository.Item2},
                {serialRepository.Item1, serialRepository.Item2},
                {banRepository.Item1, banRepository.Item2},
                {connectionRepository.Item1, connectionRepository.Item2},
                {notificationRepository.Item1, notificationRepository.Item2},
                {applicationRepository.Item1, applicationRepository.Item2},
                {questionRepository.Item1, questionRepository.Item2},
                {reportRepository.Item1, reportRepository.Item2},
                {penaltyReportRepository.Item1, penaltyReportRepository.Item2},
                {userReportRepository.Item1, userReportRepository.Item2},
                {bugRepository.Item1, bugRepository.Item2},
                {reportSubscriberRepository.Item1, reportSubscriberRepository.Item2},
                {reportCommentRepository.Item1, reportCommentRepository.Item2},
                {reportImageRepository.Item1, reportImageRepository.Item2},
                {adminActionRepository.Item1, adminActionRepository.Item2},
                {mtaLogRepository.Item1, mtaLogRepository.Item2},
                {phoneSmsRepository.Item1, phoneSmsRepository.Item2},
                {purchaseRepository.Item1, purchaseRepository.Item2},
                {orderRepository.Item1, orderRepository.Item2},
                {premiumFileRepository.Item1, premiumFileRepository.Item2},
                {gameItemRepository.Item1, gameItemRepository.Item2},
                {gameTempObjectRepository.Item1, gameTempObjectRepository.Item2},
                {gameTempInteriorRepository.Item1, gameTempInteriorRepository.Item2},
                {orderTransactionRepository.Item1, orderTransactionRepository.Item2}
            };

        public static string FindTable(Type type) => RepositoriesDictionary[type];
    }
}