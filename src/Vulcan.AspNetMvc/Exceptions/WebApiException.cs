using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulcan.AspNetMvc.Exceptions
{
    public class WebApiException : Exception
    {
        private int _ErrorCode = -1;

        public int ErrorCode
        {
            get { return _ErrorCode; }
            set { _ErrorCode = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message"></param>
        public WebApiException(string message)
            : base(message)
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public WebApiException(string message, Exception ex)
            : base(message, ex)
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="message"></param>
        public WebApiException(int errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public WebApiException(int errorCode, string message, Exception ex)
            : base(message, ex)
        {
            ErrorCode = errorCode;
        }
    }
}
