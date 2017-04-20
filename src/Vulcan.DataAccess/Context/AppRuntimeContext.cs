using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vulcan.Core;

namespace Vulcan.DataAccess.Context
{
    public static class AppRuntimeContext
    {
        private static IRuntimeContextStorage _ctxStorage;
        public static void Configure(IRuntimeContextStorage ctxStorage)
        {
            _ctxStorage = ctxStorage;

        }

        public static bool Contains(string key)
        {
            return _ctxStorage.ContainsKey(key);
        }

        public static void SetItem(string key,object item)
        {
            if (!Contains(key))
            {
                _ctxStorage.Set(key,item);
            }

        }
        public static object GetItem(string key)
        {
            object item = null;
            if (Contains(key))
            {
                item =_ctxStorage.Get(key);
            }
            return item;
        }
        public static void RemoveItem(string key)
        {
            if (Contains(key))
            {
                _ctxStorage.Remove(key);
            }
        }


    }
}
