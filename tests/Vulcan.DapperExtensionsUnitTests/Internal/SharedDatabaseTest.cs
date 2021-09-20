using AutoFixture;
using Xunit;

namespace Vulcan.DapperExtensionsUnitTests.Internal
{
    public class SharedDatabaseTest : IClassFixture<SharedDatabaseFixture>
    {
        protected SharedDatabaseTest(SharedDatabaseFixture dbFixture)
        {
            SharedDatabaseFixture = dbFixture;
            AutoFixture = new Fixture();
        }

        protected Fixture AutoFixture { get; }
        protected SharedDatabaseFixture SharedDatabaseFixture { get; }
    }
}
