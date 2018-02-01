using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using Vulcan.Core.Utilities;

namespace Vulcan.Core.Extensions
{
    public static class IDistributedCacheExtension
    {
        public static void SetObjectAsJson(this IDistributedCache cache, string key, object value, DistributedCacheEntryOptions option = default(DistributedCacheEntryOptions))
        {
            cache.SetString(key, JsonConvert.SerializeObject(value), option);
        }

        public static T GetObjectFromJson<T>(this IDistributedCache cache, string key)
        {
            var value = cache.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static void SetObjectAsByteArray<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions option =default(DistributedCacheEntryOptions))
        {
            cache.Set(key, PackUtility.Pack<T>(value), option);
        }

        public static void SetObjectAsByteArray(this IDistributedCache cache, string key, object value, DistributedCacheEntryOptions option = default(DistributedCacheEntryOptions))
        {
            Type type = value.GetType();
            cache.Set(key, PackUtility.Pack(type, value), option);
        }



        public static T GetObjectFromByteArray<T>(this IDistributedCache cache, string key)
        {
            byte[] buf = cache.Get(key);
            if(buf ==null || buf.Length == 0)
            {
                return default(T);
            }
            return PackUtility.UnPack<T>(buf);
        }
    }
}
