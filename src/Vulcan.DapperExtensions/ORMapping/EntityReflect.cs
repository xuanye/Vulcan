using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Vulcan.DapperExtensions.ORMapping.Configuration;

namespace Vulcan.DapperExtensions.ORMapping
{
    public static class EntityReflect
    {
        private static readonly ConcurrentDictionary<Type, EntityMeta> Cache =
            new ConcurrentDictionary<Type, EntityMeta>();

        private static IEntityConfigurationStore _configurationStore;

        /// <summary>
        /// Sets the entity configuration store for Fluent API configuration support.
        /// </summary>
        /// <param name="store">The entity configuration store instance.</param>
        public static void SetConfigurationStore(IEntityConfigurationStore store)
        {
            _configurationStore = store;
        }

        public static EntityMeta GetDefineInfoFromType(Type type)
        {
            if (Cache.TryGetValue(type, out var entityMeta)) return entityMeta;

            // Try to get configuration from Fluent API store first
            entityMeta = TryGetFromConfigurationStore(type);

            // Fall back to Attribute-based configuration if no Fluent API config exists
            if (entityMeta == null)
            {
                entityMeta = GetMetaFromAttributes(type);
            }

            Cache.TryAdd(type, entityMeta);

            return entityMeta;
        }

        /// <summary>
        /// Attempts to get entity metadata from the Fluent API configuration store.
        /// </summary>
        /// <param name="type">The entity type.</param>
        /// <returns>The entity metadata if found; otherwise, null.</returns>
        private static EntityMeta TryGetFromConfigurationStore(Type type)
        {
            if (_configurationStore == null)
            {
                return null;
            }

            try
            {
                // Use reflection to call the generic GetEntityMeta<T>() method
                var method = typeof(IEntityConfigurationStore).GetMethod(nameof(IEntityConfigurationStore.GetEntityMeta));
                var genericMethod = method.MakeGenericMethod(type);
                return (EntityMeta)genericMethod.Invoke(_configurationStore, null);
            }
            catch (TargetInvocationException ex) when (ex.InnerException is InvalidOperationException)
            {
                // No configuration registered for this type
                return null;
            }
        }

        /// <summary>
        /// Gets entity metadata from Attribute-based configuration.
        /// </summary>
        /// <param name="type">The entity type.</param>
        /// <returns>The entity metadata.</returns>
        private static EntityMeta GetMetaFromAttributes(Type type)
        {
            var entityMeta = new EntityMeta();

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
