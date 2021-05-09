using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Validators;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Logic.Requests.Commands
{
    public record ChangeUploadedSkinRequest : IRequest<ChangeUploadedSkinResponse>
    {
        public string OldFileId { get; init; }
        public int SkinId { get; init; }
        public IFormFile SkinFile { get; init; }
    }

    public class ChangeUploadedSkinRequestValidator : AbstractValidator<ChangeUploadedSkinRequest>
    {
        public ChangeUploadedSkinRequestValidator()
        {
            RuleFor(x => x.OldFileId).NotNull();
            RuleFor(x => x.SkinId).NotNull();
            RuleFor(x => x.SkinFile).NotNull().AllowedFileExtensionsAre(false, ".jpg", ".png")
                .MaxFileSizeIs((int) Constants.MaximumPremiumFileSize);
            ;
        }
    }
}