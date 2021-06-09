using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Requests.Queries;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Features.Responses.Queries;
using MTA.Core.Common.Helpers;
using Serilog;

namespace MTA.API.Controllers
{
    /// <summary>
    /// <b>[Authorize=Owner]</b> <br/><br/>
    /// Controller which provides article functionality
    /// </summary>
    [Authorize(Policy = Constants.OwnerPolicy)]
    public class ArticleController : BaseController
    {
        public ArticleController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// [AllowAnonymous]
        /// Get article from database 
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetArticleResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetArticle([FromQuery] GetArticleRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} fetched article #{request.ArticleId}");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// [AllowAnonymous]
        /// Get articles from database [default limit = 6] 
        /// </summary>
        [HttpGet("all")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetArticlesResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetArticles([FromQuery] GetArticlesRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} fetched articles");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Create article in database
        /// </summary>
        [HttpPost("create")]
        [ProducesResponseType(typeof(CreateArticleResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> CreateArticle([FromForm] CreateArticleRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} created new article #{response.Article?.Id}");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Update article in database
        /// </summary>
        [HttpPut("update")]
        [ProducesResponseType(typeof(UpdateArticleResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateArticle([FromForm] UpdateArticleRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} updated article #{request.ArticleId}");

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Delete article in database
        /// </summary>
        [HttpDelete("delete")]
        [ProducesResponseType(typeof(DeleteArticleResponse), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteArticle([FromQuery] DeleteArticleRequest request)
        {
            var response = await mediator.Send(request);

            Log.Information($"User #{HttpContext.GetCurrentUserId()} deleted article #{request.ArticleId}");

            return this.CreateResponse(response);
        }
    }
}