using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulcan.AspNetMvc.CacheManager
{
   public class CacheFactory
    {
       private static CacheFactory _instance = new CacheFactory();
       public static CacheFactory Instance
        {
            get
            {
                return _instance;
            }
        }

        public ICache CreateCoreCacheInstance()
        {
            return new AspNetCache();
        }

        public ICache CreateSessionCacheInstance()
        {
            return new SessionCache();
        }
    }
}
