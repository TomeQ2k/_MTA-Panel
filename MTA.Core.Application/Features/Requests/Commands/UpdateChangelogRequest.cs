using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Validation.Validators;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Features.Requests.Commands
{
    public record UpdateChangelogRequest : IRequest<UpdateChangelogResponse>
    {
        public string ChangelogId { get; init; }
        public string Title { get; init; }
        public string Content { get; init; }
        public IFormFile Image { get; init; }

        public bool IsImageDeleted { get; init; }
    }

    public class UpdateChangelogRequestValidator : AbstractValidator<UpdateChangelogRequest>
    {
        public UpdateChangelogRequestValidator()
        {
            RuleFor(x => x.ChangelogId).NotNull();
            RuleFor(x => x.Title).NotNull()
                .Length(Constants.MinimumArticleTitleLength, Constants.MaximumArticleTitleLength);
            RuleFor(x => x.Content).NotNull()
                .Length(Constants.MinimumArticleContentLength, Constants.MaximumArticleContentLength);
            RuleFor(x => x.Image).Must(x => true).MaxFileSizeIs(Constants.MaximumArticlePhotoSizeInMb)
                .AllowedFileExtensionsAre(false, ".img", ".png", ".jpg", ".jpeg", ".tiff", ".ico",
                    ".svg");
        }
    }
}