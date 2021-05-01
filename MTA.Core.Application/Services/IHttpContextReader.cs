namespace MTA.Core.Application.Services
{
    public interface IHttpContextReader
    {
        int CurrentUserId { get; }
        string CurrentUsername { get; }
        string CurrentEmail { get; }
        string ConnectionId { get; }
    }
}