using Xunit;

namespace Vulcan.DapperExtensionsUnitTests.Internal
{
    public class SharedDatabaseTest: IClassFixture<SharedDatabaseFixture>
    {
        public SharedDatabaseTest(SharedDatabaseFixture fixture) => Fixture = fixture;

        public SharedDatabaseFixture Fixture { get; }
    }
}
