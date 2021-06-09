using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses
{
    public interface IBaseResponse
    {
        bool IsSucceeded { get; init; }
        Error Error { get; init; }
    }
}