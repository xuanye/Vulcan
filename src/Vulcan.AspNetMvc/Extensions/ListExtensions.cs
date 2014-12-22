using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Vulcan.AspNetMvc.Extensions
{
    /// <summary>
    /// List排序方法扩展
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Orders the by. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="sortExpression">The sort expression.如 "StartTime desc"</param>
        /// <returns></returns>
        public static IEnumerable<T> OrderByEx<T>(this IEnumerable<T> list, string sortExpression)
        {
            if (string.IsNullOrEmpty(sortExpression))
                return list;
            string[] parts = sortExpression.Split(' ');
            bool descending = false;
            string property = "";

            if (parts.Length > 0 && parts[0] != "")
            {
                property = parts[0];

                if (parts.Length > 1)
                {
                    descending = parts[1].ToLower().Contains("esc");
                }

                PropertyInfo prop = typeof(T).GetProperty(property);

                if (prop == null)
                {
                    return list;
                }

                if (descending)
                    return list.OrderByDescending(x => prop.GetValue(x, null));
                else
                    return list.OrderBy(x => prop.GetValue(x, null));
            }

            return list;
        }
    }
}
