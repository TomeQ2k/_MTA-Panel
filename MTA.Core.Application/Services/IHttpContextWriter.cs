namespace MTA.Core.Application.Services
{
    public interface IHttpContextWriter
    {
        void AddPagination(int currentPage, int pageSize, int totalCount, int totalPages);
    }
}