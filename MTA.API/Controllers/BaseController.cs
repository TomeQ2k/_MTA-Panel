using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MTA.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        protected readonly IMediator mediator;

        protected BaseController(IMediator mediator)
        {
            this.mediator = mediator;
        }
    }
}