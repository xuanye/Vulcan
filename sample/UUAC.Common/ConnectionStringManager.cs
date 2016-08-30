using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace UUAC.Common
{
    public static class ConnectionStringManager
    {
        private static readonly Dictionary<string, string> _cache = new Dictionary<string, string>();

        public static void Configure(IConfigurationSection configSection)
        {
            foreach(var config in configSection.AsEnumerable())
            {
                if(config.Key.StartsWith("ConnectionStrings:"))
                {
                    string[] path = config.Key.Split(':');
                    string key = path[path.Length - 1];
                    if (!_cache.ContainsKey(key))
                    {
                        _cache.Add(key, config.Value);
                    }
                }
               
            }
        }

        public static string GetConnectionString(string key)
        {
            if (_cache.ContainsKey(key))
            {
                return _cache[key];
            }
            return "";
        }
    }
}
