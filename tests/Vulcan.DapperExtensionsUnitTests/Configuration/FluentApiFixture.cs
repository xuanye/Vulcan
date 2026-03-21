using System;
using Vulcan.DapperExtensions.ORMapping;
using Vulcan.DapperExtensions.ORMapping.Configuration;
using Vulcan.DapperExtensions.ORMapping.MySQL;
using Xunit;

namespace Vulcan.DapperExtensionsUnitTests.Configuration
{
    /// <summary>
    /// xUnit Collection Fixture for Fluent API tests.
    /// Ensures tests using EntityReflect share the same configuration store
    /// and execute serially to avoid static state conflicts.
    /// </summary>
    public class FluentApiFixture : IDisposable
    {
        public EntityConfigurationStore Store { get; }

        public MySQLSQLBuilder SqlBuilder { get; }

        public FluentApiFixture()
        {
            Store = new EntityConfigurationStore();
            SqlBuilder = MySQLSQLBuilder.Instance;

            // Set the configuration store for EntityReflect
            // This is shared across all tests in the same collection
            EntityReflect.SetConfigurationStore(Store);
        }

        /// <summary>
        /// Clears both EntityReflect cache and EntityConfigurationStore.
        /// Call this in test constructor to ensure test isolation.
        /// </summary>
        public void ClearAll()
        {
            EntityReflect.ClearCache();
            Store.Clear();
        }

        public void Dispose()
        {
            // Cleanup if needed
        }
    }

    /// <summary>
    /// Collection Definition for Fluent API tests.
    /// All test classes using this collection will run serially.
    /// </summary>
    [CollectionDefinition("FluentApi Collection")]
    public class FluentApiCollection : ICollectionFixture<FluentApiFixture>
    {
        // This class is just a marker, no code needed
    }
}
