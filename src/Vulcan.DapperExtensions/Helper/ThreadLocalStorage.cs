using System.Collections.Generic;
using System.Threading;
using Vulcan.DapperExtensions.Contract;

namespace Vulcan.DapperExtensions
{
    public class ThreadLocalStorage : IRuntimeContextStorage
    {
        private static readonly ThreadLocal<Dictionary<string, object>> Local =
            new ThreadLocal<Dictionary<string, object>>(() => new Dictionary<string, object>());

        public bool ContainsKey(string key)
        {
            return Local.Value.ContainsKey(key);
        }

        public object Get(string key)
        {
            return ContainsKey(key) ? Local.Value[key] : null;
        }

        public void Remove(string key)
        {
            if (ContainsKey(key))
                Local.Value.Remove(key);
        }

        public void Set(string key, object item)
        {
            if (ContainsKey(key))
                Local.Value[key] = item;
            else
                Local.Value.Add(key, item);
        }
    }
}
