using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Vulcan.DataAccess.ORMapping
{
    public class EntityReflect
    {
        private static Dictionary<Type, EntityMeta> _cache = new Dictionary<Type, EntityMeta>();
        private static object lockobject = new object();

        public static EntityMeta GetDefineInfoFromType(Type type)
        {
            EntityMeta tdefine = null;
            lock (lockobject)
            {
                if (_cache.ContainsKey(type))
                {
                    tdefine = _cache[type];
                }
            }
            if (tdefine != null)
            {
                return tdefine;
            }
            else
            {
                tdefine = new EntityMeta();
            }

            TableNameAttribute tableAttribute = (TableNameAttribute)Attribute.GetCustomAttribute(type, typeof(TableNameAttribute));
            if (tableAttribute != null)
            {
                tdefine.TableName = tableAttribute.TableName;
            }
            else
            {
                tdefine.TableName = type.FullName.Split('.').Last();
            }

            PropertyInfo[] pinfos = type.GetProperties();
            foreach (PropertyInfo p in pinfos)
            {
                Attribute[] attrs = Attribute.GetCustomAttributes(p);
                EntityColumnMeta ecmeta = new EntityColumnMeta();
                ecmeta.ColumnName = p.Name;

                foreach (Attribute cusattr in attrs)
                {
                    if (cusattr is IgnoreAttribute)
                    {
                        ecmeta = null;
                        break;
                    }
                    if (cusattr is PrimaryKeyAttribute)
                    {
                        ecmeta.PrimaryKey = true;
                    }
                    if (cusattr is MapFieldAttribute)
                    {
                        ecmeta.ColumnName = ((MapFieldAttribute)cusattr).MapFieldName;
                    }
                    if (cusattr is IdentityAttribute)
                    {
                        ecmeta.Identity = true;
                    }
                    if (cusattr is NullableAttribute)
                    {
                        ecmeta.Nullable = true;
                    }
                }
                if (ecmeta != null)
                {
                    tdefine.Columns.Add(ecmeta);
                }
            }
            lock (lockobject)
            {
                if (!_cache.ContainsKey(type))
                {
                    _cache.Add(type, tdefine);
                }
            }
            return tdefine;
        }
    }

    public class EntityMeta
    {
        public string TableName { get; set; }

        private List<EntityColumnMeta> _Columns;

        public List<EntityColumnMeta> Columns
        {
            get
            {
                if (_Columns == null)
                {
                    _Columns = new List<EntityColumnMeta>();
                }
                return _Columns;
            }
        }
    }

    public class EntityColumnMeta
    {
        public string ColumnName { get; set; }

        public bool PrimaryKey { get; set; }

        public bool Nullable { get; set; }

        public bool Identity { get; set; }
    }
}