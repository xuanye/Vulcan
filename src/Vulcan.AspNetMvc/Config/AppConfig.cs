using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulcan.AspNetMvc
{
    internal class AppConfig
    {
        /// <summary>
        /// 是否调试模式
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is debug mode; otherwise, <c>false</c>.
        /// </value>
        public static bool IsDebugMode
        {
            get
            {
                string isdebug = GetAppSetting("IsDebugMode");
                if (string.IsNullOrEmpty(isdebug))
                {
                    return false;
                }
                else
                {
                    return string.Compare(isdebug, "true", true) == 0 || isdebug == "1";
                }
            }
        }
        public static string Get(string key)
        {
            return GetAppSetting(key);

        }


        #region private
        private static string GetAppSetting(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings.Get(key);
        }
        private static string GetConnectionStrings(string key)
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings[key].ConnectionString;
        }
        #endregion
    }
}
