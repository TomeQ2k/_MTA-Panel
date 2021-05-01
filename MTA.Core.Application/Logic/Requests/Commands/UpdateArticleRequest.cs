using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Validators;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Logic.Requests.Commands
{
    public record UpdateArticleRequest : IRequest<UpdateArticleResponse>
    {
        public string ArticleId { get; init; }
        public string Title { get; init; }
        public string Content { get; init; }
        public IFormFile Image { get; init; }
        public ArticleCategoryType Category { get; init; }

        public bool IsImageDeleted { get; init; }
    }

    public class UpdateArticleRequestValidator : AbstractValidator<UpdateArticleRequest>
    {
        public UpdateArticleRequestValidator()
        {
            RuleFor(x => x.ArticleId).NotNull();
            RuleFor(x => x.Title).NotNull()
                .Length(Constants.MinimumArticleTitleLength, Constants.MaximumArticleTitleLength);
            RuleFor(x => x.Content).NotNull()
                .Length(Constants.MinimumArticleContentLength, Constants.MaximumArticleContentLength);
            RuleFor(x => x.Image).Must(x => true).MaxFileSizeIs(Constants.MaximumArticlePhotoSizeInMb)
                .AllowedFileExtensionsAre(false, ".img", ".png", ".jpg", ".jpeg", ".tiff", ".ico",
                    ".svg");
            RuleFor(x => x.Category).NotNull();
        }
    }
}