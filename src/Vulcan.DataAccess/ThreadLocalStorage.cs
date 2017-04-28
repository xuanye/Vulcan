using System.Collections.Generic;
using System.Threading;

namespace Vulcan.DataAccess
{
    public class ThreadLocalStorage : IRuntimeContextStorage
    {
        private static readonly ThreadLocal<Dictionary<string,object>> local = new ThreadLocal<Dictionary<string,object>>();
        private static object lockObj = new object();
        public bool ContainsKey(string key)
        {
            return local.Value.ContainsKey(key);
        }

        public object Get(string key)
        {
            if(ContainsKey(key)){
                return local.Value[key];
            }
            return null;
        }

        public void Remove(string key)
        {
            if(ContainsKey(key)){
               local.Value.Remove(key);
            }
        }

        public void Set(string key, object item)
        {
            if(ContainsKey(key)){
                local.Value[key] = item;
            }
            else{
                local.Value.Add(key,item);
            }
        }
    }
}
