using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Validators;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Logic.Requests.Commands
{
    public record AddCustomInteriorRequest : IRequest<AddCustomInteriorResponse>
    {
        public int EstateId { get; init; }
        public IFormFile InteriorFile { get; init; }
    }

    public class AddCustomInteriorRequestValidator : AbstractValidator<AddCustomInteriorRequest>
    {
        public AddCustomInteriorRequestValidator()
        {
            RuleFor(x => x.EstateId).NotNull();
            RuleFor(x => x.InteriorFile).NotNull().AllowedFileExtensionsAre(false, ".map")
                .MaxFileSizeIs((int) Constants.MaximumPremiumFileSize);
        }
    }
}