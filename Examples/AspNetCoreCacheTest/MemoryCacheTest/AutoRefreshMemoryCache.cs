using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace MemoryCacheTest
{
    public class AutoRefreshMemoryCache
    {
        private readonly int _refreshTimeInSeconds;
        private readonly IMemoryCache _memoryCache;
        private readonly ConcurrentDictionary<object, SemaphoreSlim> _locks = new ConcurrentDictionary<object, SemaphoreSlim>();

        public AutoRefreshMemoryCache(IMemoryCache memoryCache, int refreshInSeconds = 1)
        {
            _memoryCache = memoryCache;
            _refreshTimeInSeconds = refreshInSeconds;
        }

        public async Task<object> GetAsync(object key, Func<object, object> invalidateConfigure)
        {
            var certLock = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));
            await certLock.WaitAsync();
            try
            {
                if (!_memoryCache.TryGetValue(key, out var value))
                {
                    if (value == null)
                    {
                        value = invalidateConfigure(key);
                        _memoryCache.Set(key, value, GetMemoryCacheEntryOptions(_refreshTimeInSeconds, invalidateConfigure));
                    }
                }

                return value;
            }
            finally
            {
                certLock.Release();
            }
        }

        private MemoryCacheEntryOptions GetMemoryCacheEntryOptions(int expireInSeconds, Func<object, object> invalidateConfigure)
        {
            var expirationTime = DateTime.Now.AddSeconds(expireInSeconds);
            var expirationToken = new CancellationChangeToken(new CancellationTokenSource(TimeSpan.FromSeconds(expireInSeconds + .01)).Token);

            var options = new MemoryCacheEntryOptions();
            options.SetAbsoluteExpiration(expirationTime);
            options.AddExpirationToken(expirationToken);

            options.PostEvictionCallbacks.Add(new PostEvictionCallbackRegistration()
            {
                EvictionCallback = (key, value, reason, state) =>
                {
                    if (reason == EvictionReason.TokenExpired || reason == EvictionReason.Expired)
                    {
                        var newValue = invalidateConfigure(key);
                        _memoryCache.Set(key, newValue ?? value, GetMemoryCacheEntryOptions(_refreshTimeInSeconds, invalidateConfigure));
                    }
                }
            });

            return options;
        }
    }
}
