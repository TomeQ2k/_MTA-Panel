using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Helpers;
using MTA.Core.Application.Logic.Requests.Queries;
using MTA.Core.Application.Logic.Responses.Queries;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Application.Settings;

namespace MTA.Core.Application.Logic.Handlers.Queries
{
    public class GetHomepageStatsQuery : IRequestHandler<GetHomepageStatsRequest, GetHomepageStatsResponse>
    {
        private readonly IMtaManager mtaManager;

        public GetHomepageStatsQuery(IMtaManager mtaManager)
        {
            this.mtaManager = mtaManager;
        }

        public async Task<GetHomepageStatsResponse> Handle(GetHomepageStatsRequest request,
            CancellationToken cancellationToken)
        {
            HomepageStatsResult homepageStats = default;

            await Task.Run(() => homepageStats = mtaManager
                .CallFunction(MtaResources.WebsiteHttpFunctions, MtaFunctions.GetOnlinePlayers)
                .Replace("[", string.Empty)
                .Replace("]", string.Empty)
                .FromJSON<HomepageStatsResult>(JsonSettings.JsonSerializerOptions));

            homepageStats = homepageStats with
            {
                ServerAddress = $"{mtaManager.MtaServerSettings.Host}:{homepageStats.ServerPort}"
            };

            return new GetHomepageStatsResponse {HomepageStats = homepageStats};
        }
    }
}