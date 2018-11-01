using System.Threading;
using System.Threading.Tasks;
using MessagePack;
using MessagePack.Resolvers;
using Microsoft.Extensions.Caching.Distributed;

namespace Smartiks.Framework.Caching.Extensions
{
    public static class DistributedCacheExtensions
    {
        public static async Task<T> GetAsync<T>(this IDistributedCache distributedCache, string key, CancellationToken token = default(CancellationToken))
        {
            var bytes = await distributedCache.GetAsync(key, token);

            if (bytes == null)
                return default(T);

            return LZ4MessagePackSerializer.Deserialize<T>(bytes, ContractlessStandardResolver.Instance);
        }

        public static async Task SetAsync<T>(this IDistributedCache distributedCache, string key, T value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken))
        {
            var bytes = LZ4MessagePackSerializer.Serialize(value, ContractlessStandardResolver.Instance);

            await distributedCache.SetAsync(key, bytes, options, token);
        }
    }
}