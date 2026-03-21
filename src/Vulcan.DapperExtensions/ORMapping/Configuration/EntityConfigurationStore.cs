using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Vulcan.DapperExtensions.ORMapping.Configuration
{
    /// <summary>
    /// Provides a thread-safe store for entity configurations with caching support.
    /// </summary>
    public class EntityConfigurationStore : IEntityConfigurationStore
    {
        private readonly ConcurrentDictionary<Type, EntityMeta> _cache =
            new ConcurrentDictionary<Type, EntityMeta>();

        /// <summary>
        /// Registers an entity configuration for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The entity type being configured.</typeparam>
        /// <param name="config">The entity configuration to register.</param>
        public void RegisterConfiguration<T>(IEntityConfiguration<T> config) where T : BaseEntity
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            var builder = new EntityTypeBuilder<T>();
            config.Configure(builder);

            var entityMeta = ConvertToEntityMeta(builder);
            var entityType = typeof(T);

            _cache.AddOrUpdate(entityType, entityMeta, (key, existingValue) => entityMeta);
        }

        /// <summary>
        /// Gets the entity metadata for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The entity type to get metadata for.</typeparam>
        /// <returns>The entity metadata for the specified type.</returns>
        public EntityMeta GetEntityMeta<T>() where T : BaseEntity
        {
            var entityType = typeof(T);

            if (_cache.TryGetValue(entityType, out var entityMeta))
            {
                return entityMeta;
            }

            throw new InvalidOperationException(
                $"No configuration registered for entity type '{entityType.FullName}'. " +
                "Please register the configuration before retrieving metadata.");
        }

        /// <summary>
        /// Converts an EntityTypeBuilder configuration to EntityMeta.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="builder">The entity type builder.</param>
        /// <returns>The entity metadata.</returns>
        private static EntityMeta ConvertToEntityMeta<T>(EntityTypeBuilder<T> builder) where T : BaseEntity
        {
            var entityMeta = new EntityMeta();

            // Set table name
            entityMeta.TableName = builder.TableName ?? typeof(T).Name;

            // Get all public instance properties of the entity type
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                // Check if property is ignored
                if (builder.PropertyConfigurations.TryGetValue(property.Name, out var propertyConfig) &&
                    propertyConfig.IsIgnoredConfigured)
                {
                    continue;
                }

                var columnMeta = new EntityColumnMeta
                {
                    PropertyName = property.Name,
                    ColumnName = property.Name // Default to property name
                };

                // Apply property configuration if exists
                if (builder.PropertyConfigurations.TryGetValue(property.Name, out propertyConfig))
                {
                    // Set column name if configured
                    if (!string.IsNullOrWhiteSpace(propertyConfig.ColumnName))
                    {
                        columnMeta.ColumnName = propertyConfig.ColumnName;
                    }

                    // Set identity flag
                    columnMeta.Identity = propertyConfig.IsIdentityConfigured;

                    // Set nullable flag
                    columnMeta.Nullable = propertyConfig.IsNullableConfigured;
                }

                // Check if property is in primary key list
                if (builder.PrimaryKeyProperties.Contains(property.Name))
                {
                    columnMeta.PrimaryKey = true;
                }

                entityMeta.Columns.Add(columnMeta);
            }

            return entityMeta;
        }
    }
}