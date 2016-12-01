using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Vulcan.Core.Utilities
{
    public static class JsonUtility
    {
        public static string Serialize(object item)
        {
           return JsonConvert.SerializeObject(item);
        }

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
