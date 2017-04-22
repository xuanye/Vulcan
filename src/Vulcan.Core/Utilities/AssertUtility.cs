using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vulcan.Core.Exceptions;

namespace Vulcan.Core.Utilities
{
    public class AssertUtility
    {
        public static void IsTrue(bool condition, string errorText)
        {
            if (!condition)
            {
                throw new BizException(errorText);
            }
        }

        public static void IsFalse(bool condition, string errorText)
        {
            IsTrue(!condition, errorText);
        }

        public static void IsNull(object obj, string errorText)
        {
            IsTrue(obj == null, errorText);
        }

        public static void IsNotNull(object obj, string errorText)
        {
            IsTrue(obj != null, errorText);
        }

        public static void IsEmpty(object obj, string errorText)
        {
            IsTrue(obj == null || obj.ToString() == string.Empty, errorText);
        }

        public static void IsNotEmpty(object obj, string errorText)
        {
            IsTrue(obj != null && obj.ToString() != string.Empty, errorText);
        }

        public static void ThrowException(string errorText)
        {
            IsTrue(false, errorText);
        }
    }
}
