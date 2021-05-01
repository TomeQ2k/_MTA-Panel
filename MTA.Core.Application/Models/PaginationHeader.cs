namespace MTA.Core.Application.Models
{
    public record PaginationHeader
    (
        int CurrentPage,
        int ItemsPerPage,
        int TotalItems,
        int TotalPages
    );
}