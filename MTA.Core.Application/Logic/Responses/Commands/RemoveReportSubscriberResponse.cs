using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record RemoveReportSubscriberResponse : BaseResponse
    {
        public RemoveReportSubscriberResponse(Error error = null) : base(error)
        {
        }
    }
}