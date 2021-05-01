using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record CreateChangelogResponse : BaseResponse
    {
        public ChangelogDto Changelog { get; init; }

        public CreateChangelogResponse(Error error = null) : base(error)
        {
        }
    }
}