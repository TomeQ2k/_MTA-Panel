using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Logic.Requests.Queries;
using MTA.Core.Application.Logic.Responses.Queries;
using MTA.Core.Application.Services.ReadOnly;

namespace MTA.Core.Application.Logic.Handlers.Queries
{
    public class
        FetchPremiumLibraryFilesQuery : IRequestHandler<FetchPremiumLibraryFilesRequest,
            FetchPremiumLibraryFilesResponse>
    {
        private readonly IReadOnlyPremiumUserLibraryManager premiumUserLibraryManager;
        private readonly IMapper mapper;

        public FetchPremiumLibraryFilesQuery(IReadOnlyPremiumUserLibraryManager premiumUserLibraryManager,
            IMapper mapper)
        {
            this.premiumUserLibraryManager = premiumUserLibraryManager;
            this.mapper = mapper;
        }

        public async Task<FetchPremiumLibraryFilesResponse> Handle(FetchPremiumLibraryFilesRequest request,
            CancellationToken cancellationToken)
        {
            var result = await premiumUserLibraryManager.FetchLibraryFiles();
            return new FetchPremiumLibraryFilesResponse
            {
                SkinFiles = mapper.Map<IEnumerable<PremiumFileDto>>(result.SkinFiles),
                InteriorFiles = mapper.Map<IEnumerable<PremiumFileDto>>(result.InteriorFiles)
            };
        }
    }
}