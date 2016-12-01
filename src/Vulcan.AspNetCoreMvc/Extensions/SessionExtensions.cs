using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vulcan.Core.Utilities;

namespace Vulcan.AspNetCoreMvc.Extensions
{
    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static void SetObjectAsByteArray<T>(this ISession session, string key, T value)
        {            
            session.Set(key, PackUtility.Pack<T>(value));
        }
        public static void SetObjectAsByteArray(this ISession session, string key,object value)
        {
            Type type = value.GetType();
            session.Set(key, PackUtility.Pack(type, value));
        }
        public static T GetObjectFromByteArray<T>(this ISession session, string key)
        {
            byte[] buf = null;

            bool s = session.TryGetValue(key,out buf);
            if (s)
            {
                return PackUtility.UnPack<T>(buf);
            }
            return default(T);
        }
    }
}
