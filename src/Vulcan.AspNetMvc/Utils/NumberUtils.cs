using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vulcan.AspNetMvc.Utils
{
    public class NumberUtils
    {

        public static int ToInt(object obj, int defaultValue)
        {
            int result;
            if (obj != null && int.TryParse(obj.ToString(), out result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }

        public static int ToInt(object obj)
        {
            return ToInt(obj, 0);
        }

        public static int? ToIntNullable(object obj)
        {
            int result;
            if (obj != null && int.TryParse(obj.ToString(), out result))
            {
                return result;
            }
            return null;
        }

        public static decimal ToDecimal(object obj, decimal defaultValue)
        {
            decimal result;
            if (obj != null && decimal.TryParse(obj.ToString(), out result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }

        public static decimal ToDecimal(object obj)
        {
            return ToDecimal(obj, 0.0m);
        }

        public static double ToDouble(object obj, double defaultValue)
        {
            double result;
            if (obj != null && double.TryParse(obj.ToString(), out result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }

        public static double ToDouble(object obj)
        {
            return ToDouble(obj, 0.0);
        }

        public static sbyte ToBoolSbyte(object obj)
        {
            int intValue = ToInt(obj);
            return intValue > 0 ? (sbyte)1 : (sbyte)0;
        }

        /// <summary>
        /// 获取小数位数
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int GetDecimalPlaces(decimal val)
        {
            var str = val.ToString();
            var idx = str.IndexOf('.');
            if (idx < 0)
                return 0;
            return str.Substring(idx).Length - 1;
        }
    }
}
