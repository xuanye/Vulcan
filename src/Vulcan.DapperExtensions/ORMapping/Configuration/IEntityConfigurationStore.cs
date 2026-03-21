using System;

namespace Vulcan.DapperExtensions.ORMapping.Configuration
{
    /// <summary>
    /// Defines an interface for storing and retrieving entity configurations.
    /// </summary>
    public interface IEntityConfigurationStore
    {
        /// <summary>
        /// Registers an entity configuration for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The entity type being configured.</typeparam>
        /// <param name="config">The entity configuration to register.</param>
        void RegisterConfiguration<T>(IEntityConfiguration<T> config) where T : BaseEntity;

        /// <summary>
        /// Gets the entity metadata for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The entity type to get metadata for.</typeparam>
        /// <returns>The entity metadata for the specified type.</returns>
        EntityMeta GetEntityMeta<T>() where T : BaseEntity;
    }
}