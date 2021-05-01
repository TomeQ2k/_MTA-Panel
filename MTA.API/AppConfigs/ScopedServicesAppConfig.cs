using Microsoft.Extensions.DependencyInjection;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Filters;
using MTA.Core.Application.Logging;
using MTA.Core.Application.Models;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Application.Services.ReadOnly;
using MTA.Core.Application.SignalR;
using MTA.Infrastructure.Persistence.Logging;
using MTA.Infrastructure.Shared.Services;
using MTA.Infrastructure.Shared.SignalR;

namespace MTA.API.AppConfigs
{
    public static class ScopedServicesAppConfig
    {
        public static IServiceCollection ConfigureScopedServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRolesService, RolesService>();
            services.AddScoped<IResetPasswordManager, ResetPasswordManager>();
            services.AddScoped<IUserTokenGenerator, UserTokenGenerator>();
            services.AddScoped<IJwtAuthorizationTokenGenerator, JwtAuthorizationTokenGenerator>();
            services.AddScoped<IHashGenerator, HashGenerator>();
            services.AddScoped<ISerialService, SerialService>();
            services.AddScoped<ICryptoService, CryptoService>();
            services.AddScoped<ITokenCleaner, TokenCleaner>();
            services.AddScoped<IAccountManager, AccountManager>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<ICharacterService, CharacterService>();
            services.AddScoped<ICharacterManager, CharacterManager>();
            services.AddScoped<IChangelogService, ChangelogService>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<IBanService, BanService>();
            services.AddScoped<INotifier, Notifier>();
            services.AddScoped<IStatsService<ServerStatsResult>, ServerStatsService>();
            services.AddScoped<IStatsService<PlayersActivityStatsResult>, PlayersActivityStatsService>();
            services.AddScoped<IStatsService<FactionsStatsResult>, FactionsStatsService>();
            services.AddScoped<IStatsService<AdminsActivityStatsResult>, AdminsActivityStatsService>();
            services.AddScoped<IStatsService<MoneyStatsResult>, MoneyStatsService>();
            services.AddScoped<IAuthValidationService, AuthValidationService>();
            services.AddScoped<INotifierValidationService, NotifierValidationService>();
            services.AddScoped(typeof(IHubManager<>), typeof(HubManager<>));
            services.AddScoped<IConnectionManager, ConnectionManager>();
            services.AddScoped<IRPTestManager, RPTestManager>();
            services.AddScoped<IAdminActionService, AdminActionService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IReportManager, ReportManager>();
            services.AddScoped<IReportCommentService, ReportCommentService>();
            services.AddScoped<IReportSubscriberService, ReportSubscriberService>();
            services.AddScoped<IReportImageService, ReportImageService>();
            services.AddScoped<IReportValidationService, ReportValidationService>();
            services.AddScoped<IReportValidationHub, ReportValidationHub>();
            services.AddScoped<ILogReader, LogReader>();
            services.AddScoped<ILogReaderHelper, LogReaderHelper>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPurchaseService, PurchaseService>();
            services.AddScoped<IPremiumAccountManager, PremiumAccountManager>();
            services.AddScoped<IPremiumUserLibraryManager, PremiumUserLibraryManager>();
            services.AddScoped<ITempDatabaseCleaner, TempDatabaseCleaner>();
            services.AddScoped<ICustomInteriorManager, CustomInteriorManager>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IDonationManager, DonationManager>();
            services.AddScoped<IOrderTransactionService, OrderTransactionService>();
            services.AddScoped<IRewardReferrerSystem, RewardReferrerSystem>();

            services.AddScoped<IReadOnlyAccountManager, AccountManager>();
            services.AddScoped<IReadOnlyUserService, UserService>();
            services.AddScoped<IReadOnlyCharacterService, CharacterService>();
            services.AddScoped<IReadOnlyArticleService, ArticleService>();
            services.AddScoped<IReadOnlySerialService, SerialService>();
            services.AddScoped<IReadOnlyChangelogService, ChangelogService>();
            services.AddScoped<IReadOnlyNotifier, Notifier>();
            services.AddScoped<IReadOnlyRPTestManager, RPTestManager>();
            services.AddScoped<IReadOnlyAdminActionService, AdminActionService>();
            services.AddScoped<IReadOnlyBanService, BanService>();
            services.AddScoped<IReadOnlyReportService, ReportService>();
            services.AddScoped<IReadOnlyReportManager, ReportManager>();
            services.AddScoped<IReadOnlyOrderService, OrderService>();
            services.AddScoped<IReadOnlyPurchaseService, PurchaseService>();
            services.AddScoped<IReadOnlyPremiumUserLibraryManager, PremiumUserLibraryManager>();

            services.RegisterAllTypes<IReportMember>(new[] {typeof(IReportMember).Assembly});

            services.AddScoped<PremiumFilter>();

            return services;
        }
    }
}