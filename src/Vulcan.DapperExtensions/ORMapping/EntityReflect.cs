using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Vulcan.DapperExtensions.ORMapping
{
    public static class EntityReflect
    {
        private static readonly ConcurrentDictionary<Type, EntityMeta> Cache =
            new ConcurrentDictionary<Type, EntityMeta>();

        public static EntityMeta GetDefineInfoFromType(Type type)
        {
            if (Cache.TryGetValue(type, out var entityMeta)) return entityMeta;


            entityMeta = new EntityMeta();


            var typeInfo = type.GetTypeInfo();

            var tableAttribute = typeInfo.GetCustomAttribute<TableNameAttribute>();

            entityMeta.TableName = tableAttribute != null ? tableAttribute.TableName : type.FullName?.Split('.').Last();

            foreach (var p in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var attrs = p.GetCustomAttributes();


                var ecMeta = new EntityColumnMeta
                {
                    ColumnName = p.Name,
                    PropertyName = p.Name
                };

                foreach (var cusAttr in attrs)
                {
                    if (cusAttr is IgnoreAttribute)
                    {
                        ecMeta = null;
                        break;
                    }

                    switch (cusAttr)
                    {
                        case PrimaryKeyAttribute _:
                            ecMeta.PrimaryKey = true;
                            break;
                        case MapFieldAttribute mfAttr:
                            ecMeta.ColumnName = mfAttr.MapFieldName;
                            break;
                        case IdentityAttribute _:
                            ecMeta.Identity = true;
                            break;
                        case NullableAttribute _:
                            ecMeta.Nullable = true;
                            break;
                    }
                }

                if (ecMeta != null) entityMeta.Columns.Add(ecMeta);
            }

            Cache.TryAdd(type, entityMeta);

            return entityMeta;
        }
    }

    public class EntityMeta
    {
        public string TableName { get; set; }
        public List<EntityColumnMeta> Columns { get; } = new List<EntityColumnMeta>();
    }

    public class EntityColumnMeta
    {
        /// <summary>
        ///     column's name in db
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        ///     PropertyName in csharp class
        /// </summary>
        public string PropertyName { get; set; }

        public bool PrimaryKey { get; set; }

        public bool Nullable { get; set; }

        public bool Identity { get; set; }
    }
}
