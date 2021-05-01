using FluentValidation;
using MediatR;
using MTA.Core.Application.Logic.Responses.Commands;

namespace MTA.Core.Application.Logic.Requests.Commands
{
    public record DeleteSerialRequest : IRequest<DeleteSerialResponse>
    {
        public int SerialId { get; init; }
    }

    public class DeleteSerialRequestValidator : AbstractValidator<DeleteSerialRequest>
    {
        public DeleteSerialRequestValidator()
        {
            RuleFor(x => x.SerialId).NotNull();
        }
    }
}