using AutoFixture;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Vulcan.DapperExtensionsUnitTests.Internal
{
    [CollectionDefinition("Async Database collection")]
    public class AsyncSharedDatabaseTest : ICollectionFixture<AsyncSharedDatabaseFixture>
    {
     
    }
}
