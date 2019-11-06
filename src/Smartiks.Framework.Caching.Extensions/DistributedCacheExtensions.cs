using MessagePack;
using MessagePack.Resolvers;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading;
using System.Threading.Tasks;

namespace Smartiks.Framework.Caching.Extensions
{
    public static class DistributedCacheExtensions
    {
        public static async Task<T> GetAsync<T>(this IDistributedCache distributedCache, string key, CancellationToken token = default)
        {
            var bytes = await distributedCache.GetAsync(key, token);

            if (bytes == null)
                return default;

            return LZ4MessagePackSerializer.Deserialize<T>(bytes, ContractlessStandardResolver.Instance);
        }

        public static async Task SetAsync<T>(this IDistributedCache distributedCache, string key, T value, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            var bytes = LZ4MessagePackSerializer.Serialize(value, ContractlessStandardResolver.Instance);

            await distributedCache.SetAsync(key, bytes, options, token);
        }
    }
}