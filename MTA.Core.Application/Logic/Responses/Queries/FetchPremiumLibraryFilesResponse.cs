using System.Collections.Generic;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Queries
{
    public record FetchPremiumLibraryFilesResponse : BaseResponse
    {
        public IEnumerable<PremiumFileDto> SkinFiles { get; init; }
        public IEnumerable<PremiumFileDto> InteriorFiles { get; init; }

        public FetchPremiumLibraryFilesResponse(Error error = null) : base(error)
        {
        }
    }
}