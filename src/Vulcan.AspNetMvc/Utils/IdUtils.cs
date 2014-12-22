using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulcan.AspNetMvc.Utils
{
    public class IdUtils
    {
        public static String NewGuid()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 生成32唯一ID
        /// </summary>
        /// <returns></returns>
        public static string GetNewUniqueId()
        {
            return NewGuid().Replace("-", "");
        }
    }
}
