using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Helpers;
using MTA.Core.Application.Services;
using MTA.Core.Application.SignalR;
using MTA.Core.Application.SignalR.Hubs;
using MTA.Core.Common.Enums;
using Serilog;

namespace MTA.Core.Application.Features.Handlers.Commands
{
    public class DonateServerCommand : IRequestHandler<DonateServerRequest, DonateServerResponse>
    {
        private readonly IDonationManager donationManager;
        private readonly IHttpContextReader httpContextReader;
        private readonly INotifier notifier;
        private readonly IHubManager<NotifierHub> hubManager;
        private readonly IRewardReferrerSystem rewardReferrerSystem;
        private readonly IMapper mapper;

        public DonateServerCommand(IDonationManager donationManager, IHttpContextReader httpContextReader,
            INotifier notifier, IHubManager<NotifierHub> hubManager, IRewardReferrerSystem rewardReferrerSystem,
            IMapper mapper)
        {
            this.donationManager = donationManager;
            this.httpContextReader = httpContextReader;
            this.notifier = notifier;
            this.hubManager = hubManager;
            this.rewardReferrerSystem = rewardReferrerSystem;
            this.mapper = mapper;
        }

        public async Task<DonateServerResponse> Handle(DonateServerRequest request, CancellationToken cancellationToken)
        {
            var donateServerResult = request.DonationType switch
            {
                DonationType.ThreeHundredSeventyFivePLN =>
                    await donationManager.DonateServerDlcBrain(request.TokenCode),
                _ => await donationManager.DonateServer(request.DonationType, request.TokenCode)
            } ?? throw new ServerException("Donating server failed");

            if (donateServerResult.IsSucceeded)
            {
                var notification = await notifier.Push(
                    "You have donated server successfully, credit points will be added automatically in the next 24h",
                    httpContextReader.CurrentUserId);

                await hubManager.Invoke(SignalrActions.NOTIFICATION_RECEIVED, httpContextReader.CurrentUserId,
                    mapper.Map<NotificationDto>(notification));

                if (request.DonationType != DonationType.ThreeHundredSeventyFivePLN)
                {
                    var rewardReferrerResult = await rewardReferrerSystem.Reward(RewardReferrerType.ServerDonated,
                        DonationDictionary.CalculateCredits(request.DonationType));

                    if (rewardReferrerResult.IsRewarded)
                    {
                        Log.Information(
                            $"Referrer #{rewardReferrerResult.ReferrerId} has been rewarded with {rewardReferrerResult.RewardCredits} credit points");

                        var rewardReferrerNotification = await notifier.Push(
                            $"[REFERRER SYSTEM]: You have been rewarded with {rewardReferrerResult.RewardCredits} credit points",
                            rewardReferrerResult.ReferrerId);

                        await hubManager.Invoke(SignalrActions.NOTIFICATION_RECEIVED, rewardReferrerResult.ReferrerId,
                            mapper.Map<NotificationDto>(rewardReferrerNotification));
                    }
                }

                return (DonateServerResponse) new DonateServerResponse
                {
                    IsSucceeded = donateServerResult.IsSucceeded,
                    CreditsAdded = donateServerResult.CreditsAdded
                }.LogInformation(
                    $"User #{httpContextReader.CurrentUserId} donated server for: {(int) request.DonationType} PLN");
            }

            throw new ServerException("Donating server failed");
        }
    }
}