using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Requests.Queries;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Logic.Responses.Queries;
using MTA.Core.Common.Helpers;

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
        /// <b>[AllowAnonymous]</b> <br/><br/>
        /// Get article from database 
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetArticleResponse), 200)]
        public async Task<IActionResult> GetArticle([FromQuery] GetArticleRequest request)
        {
            var response = await mediator.Send(request);

            return this.CreateResponse(response);
        }

        /// <summary>
        /// <b>[AllowAnonymous]</b> <br/><br/>
        /// Get articles from database [default limit = 6] 
        /// </summary>
        [HttpGet("all")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetArticlesResponse), 200)]
        public async Task<IActionResult> GetArticles([FromQuery] GetArticlesRequest request)
        {
            var response = await mediator.Send(request);

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Create article in database
        /// </summary>
        [HttpPost("create")]
        [ProducesResponseType(typeof(CreateArticleResponse), 200)]
        public async Task<IActionResult> CreateArticle([FromForm] CreateArticleRequest request)
        {
            var response = await mediator.Send(request);

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Update article in database
        /// </summary>
        [HttpPut("update")]
        [ProducesResponseType(typeof(UpdateArticleResponse), 200)]
        public async Task<IActionResult> UpdateArticle([FromForm] UpdateArticleRequest request)
        {
            var response = await mediator.Send(request);

            return this.CreateResponse(response);
        }

        /// <summary>
        /// Delete article in database
        /// </summary>
        [HttpDelete("delete")]
        [ProducesResponseType(typeof(DeleteArticleResponse), 200)]
        public async Task<IActionResult> DeleteArticle([FromQuery] DeleteArticleRequest request)
        {
            var response = await mediator.Send(request);

            return this.CreateResponse(response);
        }
    }
}