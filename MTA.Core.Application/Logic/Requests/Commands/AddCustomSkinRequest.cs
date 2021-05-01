using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Validators;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Logic.Requests.Commands
{
    public record AddCustomSkinRequest : IRequest<AddCustomSkinResponse>
    {
        public int SkinId { get; init; }
        public int CharacterId { get; init; }
        public IFormFile SkinFile { get; init; }
    }

    public class AddCustomSkinRequestValidator : AbstractValidator<AddCustomSkinRequest>
    {
        public AddCustomSkinRequestValidator()
        {
            RuleFor(x => x.SkinId).NotNull();
            RuleFor(x => x.CharacterId).NotNull();
            RuleFor(x => x.SkinFile).NotNull().AllowedFileExtensionsAre(false, ".jpg", ".png")
                .MaxFileSizeIs((int) Constants.MaximumPremiumFileSize);
        }
    }
}