using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vulcan.AspNetMvc.Exceptions;


namespace Vulcan.AspNetMvc.Utils
{
    public class AssertUtils
    {
        public static void IsTrue(bool condition, String errorText)
        {
            if (!condition)
            {
                throw new BizException(errorText);
            }
        }

        public static void IsFalse(bool condition, String errorText)
        {
            IsTrue(!condition, errorText);
        }

        public static void IsNull(Object obj, String errorText)
        {
            IsTrue(obj == null, errorText);
        }

        public static void IsNotNull(Object obj, String errorText)
        {
            IsTrue(obj != null, errorText);
        }

        public static void IsEmpty(Object obj, String errorText)
        {
            IsTrue(obj == null || obj.ToString() == string.Empty, errorText);
        }

        public static void IsNotEmpty(Object obj, String errorText)
        {
            IsTrue(obj != null && obj.ToString() != string.Empty, errorText);
        }

        public static void ThrowException(string errorText)
        {
            IsTrue(false, errorText);
        }
    }
}
