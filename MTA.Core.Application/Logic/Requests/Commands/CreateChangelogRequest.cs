using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Validators;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Logic.Requests.Commands
{
    public record CreateChangelogRequest : IRequest<CreateChangelogResponse>
    {
        public string Title { get; init; }
        public string Content { get; init; }
        public IFormFile Image { get; init; }
    }

    public class CreateChangelogRequestValidator : AbstractValidator<CreateChangelogRequest>
    {
        public CreateChangelogRequestValidator()
        {
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