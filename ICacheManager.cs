using System;

namespace AspNetCacheDemo
{
    public interface ICacheManager
    {
        void RemoveAll();

        void AddOrUpdate(CacheItem cacheItem);

        void Remove(CacheItem cacheItem);

        void Remove(string key);

        T Get<T>(string key) where T : class;

        T GetWithFallback<T>(string key, int cacheDurationMinutes, Func<T> fallbackQueryFunc) where T : class;
    }
}
