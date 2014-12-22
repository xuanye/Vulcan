using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulcan.AspNetMvc.CacheManager
{
    /// <summary>
    /// session级别的缓存
    /// </summary>
    public class SessionCache : ICache
    {
        private static object _lockobject = new object();
        public int Count
        {
            get { return SessionManager.GetSessionCount(); }
        }

        public IList<string> Keys
        {
            get { return SessionManager.GetSessionKeys(); }
        }

        public object Get(string key)
        {
            return SessionManager.Get(key);
        }

        public void Remove(string key)
        {
            lock (_lockobject)
            {
                if (Get(key) !=null)
                {
                    SessionManager.Remove(key);
                }
                
            }
            
        }

        public void Clear()
        {
            lock (_lockobject)
            {
                SessionManager.RemoveAll();
            }
        }

        public void Insert(string key, object value)
        {
            lock (_lockobject)
            {
                if (Get(key) != null)
                {
                    SessionManager.Remove(key);                    
                }
                SessionManager.Add(key, value);
            }
            
        }

        public void Insert(string key, object value, TimeSpan timeToLive)
        {
            Insert(key, value);
        }
    }
}
