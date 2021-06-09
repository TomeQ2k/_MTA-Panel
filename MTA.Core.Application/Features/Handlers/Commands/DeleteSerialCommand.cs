using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Services;

namespace MTA.Core.Application.Features.Handlers.Commands
{
    public class DeleteSerialCommand : IRequestHandler<DeleteSerialRequest, DeleteSerialResponse>
    {
        private readonly ISerialService serialService;

        public DeleteSerialCommand(ISerialService serialService)
        {
            this.serialService = serialService;
        }

        public async Task<DeleteSerialResponse> Handle(DeleteSerialRequest request, CancellationToken cancellationToken)
            => await serialService.DeleteSerial(request.SerialId)
                ? new DeleteSerialResponse()
                : throw new DatabaseException("Cannot delete this serial");
    }
}