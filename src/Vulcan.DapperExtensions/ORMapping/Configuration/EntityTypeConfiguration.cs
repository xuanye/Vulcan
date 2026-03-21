using System;

namespace Vulcan.DapperExtensions.ORMapping.Configuration
{
    /// <summary>
    /// A base class for configuring an entity type using the Fluent API.
    /// Inherit from this class to define entity configurations that can be overridden.
    /// </summary>
    /// <typeparam name="T">The entity type being configured.</typeparam>
    public abstract class EntityTypeConfiguration<T> : IEntityConfiguration<T> where T : BaseEntity
    {
        /// <summary>
        /// Configures the entity type using the specified builder.
        /// Override this method in a derived class to configure the entity.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity type.</param>
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            // Default implementation does nothing.
            // Derived classes should override this method to configure the entity.
        }
    }
}
