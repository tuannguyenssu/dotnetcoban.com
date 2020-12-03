using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace MemoryCacheTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //await TestExpireCacheAsync();
            var cache = new MemoryCache(new MemoryCacheOptions()
            {
                ExpirationScanFrequency = new TimeSpan(0, 0, 1)
            });
            var autoRefreshCache = new AutoRefreshMemoryCache(cache, 2);
            var key = "Key";
            for (int i = 0; i < 1000; i++)
            {
                var value = await autoRefreshCache.GetAsync(key, x => Guid.NewGuid().ToString());
                Console.WriteLine(value);
                await Task.Delay(TimeSpan.FromSeconds(1));
            }

            Console.ReadKey();
        }

        public static async Task TestExpireCacheAsync()
        {
            var cache = new MemoryCache(new MemoryCacheOptions()
            {
                ExpirationScanFrequency = new TimeSpan(0, 0, 1)
            });

            var key = "Key";
            var value = "Value";
            var newValue = "New Value";


            int expirationSeconds = 1;
            var expirationTime = DateTime.Now.AddSeconds(expirationSeconds);
            var expirationToken = new CancellationChangeToken(
                new CancellationTokenSource(TimeSpan.FromSeconds(expirationSeconds + .01)).Token);

            var options = new MemoryCacheEntryOptions();
            options.SetAbsoluteExpiration(expirationTime);
            options.AddExpirationToken(expirationToken);
            options.PostEvictionCallbacks.Add(new PostEvictionCallbackRegistration()
            {
                EvictionCallback = (subKey, subValue, reason, state) =>
                {
                    if (reason == EvictionReason.TokenExpired || reason == EvictionReason.Expired)
                    {
                        cache.Set(key, newValue);
                    }
                }
            });

            cache.Set(key, value, options);
            Console.WriteLine(cache.Get(key));
            await Task.Delay(TimeSpan.FromSeconds(1));
            var result = cache.Get(key);
            Console.WriteLine(result);
        }
    }
}
