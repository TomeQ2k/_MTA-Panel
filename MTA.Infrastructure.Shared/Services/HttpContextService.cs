using Microsoft.AspNetCore.Http;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Services;

namespace MTA.Infrastructure.Shared.Services
{
    public class HttpContextService : IHttpContextService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public HttpContextService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public int CurrentUserId => httpContextAccessor.HttpContext.GetCurrentUserId();
        public string CurrentUsername => httpContextAccessor.HttpContext.GetCurrentUsername();
        public string CurrentEmail => httpContextAccessor.HttpContext.GetCurrentEmail();
        public string ConnectionId => httpContextAccessor.HttpContext.GetConnectionId();

        public void AddPagination(int currentPage, int pageSize, int totalCount, int totalPages)
            => httpContextAccessor.HttpContext.Response.AddPagination(currentPage, pageSize, totalCount, totalPages);
    }
}