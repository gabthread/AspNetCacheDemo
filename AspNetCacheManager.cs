using System;
using System.Web;
using System.Web.Caching;


namespace AspNetCacheDemo
{
    public class AspNetCacheManager : ICacheManager
    {
        private Cache cache;


        public AspNetCacheManager()
        {
            this.cache = HttpRuntime.Cache;
        }

        public void RemoveAll()
        {
            var enumerator = HttpRuntime.Cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                this.cache.Remove((string)enumerator.Key);
            }
        }

        public void AddOrUpdate(CacheItem cacheItem)
        {
            if (cacheItem.Key == null || cacheItem.Value == null)
                return;

            cacheItem.AddedToCacheOn = DateTime.Now;
            this.cache.Insert(cacheItem.Key, cacheItem.Value, (CacheDependency)null, DateTime.UtcNow.AddMinutes(cacheItem.CacheDuration.TotalMinutes), Cache.NoSlidingExpiration);
        }

        public void Remove(CacheItem cacheItem)
        {
            this.cache.Remove(cacheItem.Key);
        }

        public void Remove(string key)
        {
            this.cache.Remove(key);
        }

        public T Get<T>(string key) where T : class 
        {
            return  this.cache.Get(key) as T;
        }

        public T GetWithFallback<T>(string key, int cacheDurationMinutes, Func<T> fallbackQueryFunc) where T : class
        {
            T obj1 = this.Get<T>(key);

            if ((object)obj1 != null)
                return obj1;

            T obj2 = fallbackQueryFunc();

            this.AddOrUpdate(new CacheItem()
            {
                CacheDuration = new TimeSpan(0, cacheDurationMinutes, 0),
                Key = key,
                Value = (object)obj2
            });

            return obj2;
        }

        

       
    }
}
