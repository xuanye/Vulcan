using System;

namespace Vulcan.DapperExtensions.ORMapping.Configuration
{
    /// <summary>
    /// Defines an interface for configuring an entity type using the Fluent API.
    /// Implementations of this interface are used to configure entity mappings.
    /// </summary>
    /// <typeparam name="T">The entity type being configured.</typeparam>
    public interface IEntityConfiguration<T> where T : BaseEntity
    {
        /// <summary>
        /// Configures the entity type using the specified builder.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity type.</param>
        void Configure(EntityTypeBuilder<T> builder);
    }
}
