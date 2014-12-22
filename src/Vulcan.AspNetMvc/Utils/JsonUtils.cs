using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Text;

namespace Vulcan.AspNetMvc.Utils
{
    public static class JsonUtils
    {
        public static T FromJson<T>(string str)
        {
            if (str == null) str = string.Empty;
            return str.FromJson<T>();
        }

        public static string ToJson(Object value)
        {
            if (value == null) return string.Empty;
            return value.ToJson();
        }
    }
}
