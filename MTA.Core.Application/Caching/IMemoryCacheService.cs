namespace MTA.Core.Application.Caching
{
    public interface IMemoryCacheService<T> : ICacheService<T> where T : class, new()
    {
    }
}