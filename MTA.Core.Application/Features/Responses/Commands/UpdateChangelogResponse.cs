using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record UpdateChangelogResponse : BaseResponse
    {
        public ChangelogDto Changelog { get; init; }

        public UpdateChangelogResponse(Error error = null) : base(error)
        {
        }
    }
}