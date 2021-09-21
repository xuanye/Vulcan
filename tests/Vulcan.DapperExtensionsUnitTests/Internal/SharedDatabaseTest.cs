using AutoFixture;
using Xunit;

namespace Vulcan.DapperExtensionsUnitTests.Internal
{
    [CollectionDefinition("Database collection")]
    public class SharedDatabaseTest : ICollectionFixture<SharedDatabaseFixture>
    {
       
    }
}
