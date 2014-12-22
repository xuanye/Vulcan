using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Vulcan.AspNetMvc.CacheManager
{
    public class SessionManager
    {
        public static void Add(string key, object obj)
        {
            HttpContext.Current.Session.Add(key, obj);
        }
        public static object Get(string key)
        {
            return HttpContext.Current.Session[key];
        }

        public static void Remove(string key)
        {
            HttpContext.Current.Session.Remove(key);
        }

        public static void RemoveAll()
        {
            HttpContext.Current.Session.RemoveAll();
        }

        public static int GetSessionCount()
        {

            return HttpContext.Current.Session.Count;
        }

        public static IList<string> GetSessionKeys()
        {
            IList<string> result = new List<string>();
            ICollection list = HttpContext.Current.Session.Keys;
            if (list != null)
            {
                foreach (var item in list)
                {
                    if (item != null)
                    {
                        result.Add(item.ToString());
                    }
                }
            }
            return result;

        }
    }
}
