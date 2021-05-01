using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Validators;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Logic.Requests.Commands
{
    public class ChangeUploadedInteriorRequest : IRequest<ChangeUploadedInteriorResponse>
    {
        public string OldFileId { get; init; }
        public IFormFile InteriorFile { get; init; }
    }

    public class ChangeUploadedInteriorFileRequestValidator : AbstractValidator<ChangeUploadedInteriorRequest>
    {
        public ChangeUploadedInteriorFileRequestValidator()
        {
            RuleFor(x => x.OldFileId).NotNull();
            RuleFor(x => x.InteriorFile).NotNull().AllowedFileExtensionsAre(false, ".map")
                .MaxFileSizeIs((int) Constants.MaximumPremiumFileSize);
            ;
        }
    }
}