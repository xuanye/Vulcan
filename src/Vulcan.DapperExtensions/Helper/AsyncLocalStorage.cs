using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Vulcan.DapperExtensions.Contract;

namespace Vulcan.DapperExtensions
{
    public class AsyncLocalStorage : IRuntimeContextStorage
    {
        public static Dictionary<string, object> LocalValue
        {
            get => Local.Value;
            set => Local.Value = value;
        }

        private static readonly AsyncLocal<Dictionary<string, object>> Local =
            new AsyncLocal<Dictionary<string, object>>();

        public bool ContainsKey(string key)
        {
            return LocalValue.ContainsKey(key);
        }

        public object Get(string key)
        {
            return ContainsKey(key) ? LocalValue[key] : null;
        }

        public void Remove(string key)
        {
            if (ContainsKey(key))
                LocalValue.Remove(key);
        }

        public void Set(string key, object item)
        {
            if (ContainsKey(key))
                LocalValue[key] = item;
            else
                LocalValue.Add(key, item);
        }
    }
}
