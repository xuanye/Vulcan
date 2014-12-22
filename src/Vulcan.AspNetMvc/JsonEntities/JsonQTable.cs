using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;


namespace Vulcan.AspNetMvc.JsonEntities
{
    public class JsonQTable
    {
        public JsonQTable()
        {
        }

         public JsonQTable(
            int pageIndex, int totalCount, IList<JsonQRow> data)
        {
            page = pageIndex;
            total = totalCount;
            rows = data;
        }
        public int page { get; set; }
        public int total { get; set; }
        public IList<JsonQRow> rows { get; set; }

        /// <summary>
        /// 自定义扩展信息
        /// </summary>
        public object  DataExtend { get; set;}

        public static JsonQTable ConvertFromList<T>(List<T> list, string key,string[] cols) where T : class
        {
            JsonQTable data = new JsonQTable();
            data.page = 1;
            if (list != null)
            {
                data.total = list.Count;
                data.rows = new List<JsonQRow>();
                foreach (T t in list)
                {
                    JsonQRow row = new JsonQRow();
                    row.id = getValue<T>(t, key);
                    row.cell = new List<string>();
                    foreach (string col in cols)
                    {
                        row.cell.Add(getValue<T>(t, col));
                    }
                    data.rows.Add(row);
                }
            }
            else
            {
                data.total = 0;
            }
            return data;
        }

        public static JsonQTable ConvertFromList<T>(List<T> list, string key, string[] cols, Func<string, bool> checkExtP, Func<string, T, string> exciteExtP) where T : class
        {
            JsonQTable data = new JsonQTable();
            data.page = 1;
            if (list != null)
            {
                data.total = list.Count;
                data.rows = new List<JsonQRow>();
                foreach (T t in list)
                {
                    JsonQRow row = new JsonQRow();
                    row.id = getValue<T>(t, key);
                    row.cell = new List<string>();
                    foreach (string col in cols)
                    {
                        if (checkExtP(col))
                        {
                            row.cell.Add(exciteExtP(col, t));
                        }
                        else
                        {
                            row.cell.Add(getValue<T>(t, col));
                        }
                    }
                    data.rows.Add(row);
                }
            }
            else
            {
                data.total = 0;
            }
            return data;
        }

        private static string getValue<T>(T t, string pname) where T : class
        {
            Type type = t.GetType();
            PropertyInfo pinfo = type.GetProperty(pname);
            if (pinfo != null)
            {
                object v = pinfo.GetValue(t, null);
                return v != null ? v.ToString() : "";
            }
            return "";
        }
        public static JsonQTable ConvertFromPagedList<T>(List<T> pageList,int pageIndex,int total, string key, string[] cols, Func<string, bool> checkExtP, Func<string, T, string> excuteExtP) where T : class
        {
            JsonQTable data = new JsonQTable();
            data.page = pageIndex + 1;
            data.total = total;
            data.rows = new List<JsonQRow>();
            foreach (T t in pageList)
            {
                JsonQRow row = new JsonQRow();
                row.id = getValue<T>(t, key);
               
                row.cell = new List<string>();
                foreach (string col in cols)
                {
                    if (checkExtP(col))
                    {
                        row.cell.Add(excuteExtP(col, t));
                    }
                    else
                    {
                        row.cell.Add(getValue<T>(t, col));
                    }
                }
                data.rows.Add(row);
            }
            return data;
        }

        public static JsonQTable ConvertFromPagedList<T>(List<T> pageList, int pageIndex, int total, string key, string[] cols) where T : class
        {
            JsonQTable data = new JsonQTable();
            data.page = pageIndex + 1;
            data.total = total;
            data.rows = new List<JsonQRow>();
            foreach (T t in pageList)
            {
                JsonQRow row = new JsonQRow();
                row.id = getValue<T>(t, key);
                row.cell = new List<string>();
                foreach (string col in cols)
                {
                    row.cell.Add(getValue<T>(t, col));
                }
                data.rows.Add(row);
            }
            return data;
        }
    }

    public class JsonQRow
    {
        public string id { get; set; }
        public List<string> cell { get; set; }
    }
}
