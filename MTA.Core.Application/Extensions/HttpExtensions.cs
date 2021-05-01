using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Extensions
{
    public static class HttpExtensions
    {
        public static int GetCurrentUserId(this HttpContext httpContext)
            => int.Parse(httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty);

        public static string GetCurrentUsername(this HttpContext httpContext)
            => httpContext.User.FindFirst(ClaimTypes.Name)?.Value;

        public static string GetCurrentEmail(this HttpContext httpContext)
            => httpContext.User.FindFirst(ClaimTypes.Email)?.Value;

        public static bool IsAuthenticated(this HttpContext httpContext)
            => httpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null;

        public static string GetConnectionId(this HttpContext httpContext)
            => httpContext.Connection.Id;

        public static void AddApplicationError(this HttpResponse response, string errorMessage)
        {
            response.Headers.Add("Application-Error", errorMessage);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static void AddPagination(this HttpResponse response, int currentPage, int pageSize, int totalCount,
            int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, pageSize, totalCount, totalPages);

            response.Headers.Add("Pagination", paginationHeader.ToJSON());
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }

        public static HttpClient AddRequestHeaders(this HttpClient httpClient, string mediaType = "application/json")
        {
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

            return httpClient;
        }
    }
}