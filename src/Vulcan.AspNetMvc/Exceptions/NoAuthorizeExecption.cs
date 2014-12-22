using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulcan.AspNetMvc.Exceptions
{
    /// <summary>
    /// 没有登录的异常
    /// </summary>
    [Serializable]
    public class NoAuthorizeExecption : Exception
    {
        public NoAuthorizeExecption()
            : base("你没有登录，该页面必须登录后访问！")
        {

        }
    }
}
