using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record AddReportSubscriberResponse : BaseResponse
    {
        public ReportSubscriberDto ReportSubscriber { get; init; }

        public AddReportSubscriberResponse(Error error = null) : base(error)
        {
        }
    }
}