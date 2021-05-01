using Microsoft.AspNetCore.Mvc;
using MTA.Core.Application.Logic.Responses;

namespace MTA.Core.Application.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult CreateResponse(this ControllerBase controller, IBaseResponse response)
            => response.IsSucceeded
                ? controller.Ok(response)
                : controller.StatusCode((int) response.Error.StatusCode, response);
    }
}