using System.Collections.Generic;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Queries
{
    public record GetArticlesResponse : BaseResponse
    {
        public IEnumerable<ArticleDto> Articles { get; init; }

        public GetArticlesResponse(Error error = null) : base(error)
        {
        }
    }
}