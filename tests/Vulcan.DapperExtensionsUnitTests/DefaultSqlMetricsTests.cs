using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.AutoMock;
using Vulcan.DapperExtensions;
using Xunit;

namespace Vulcan.DapperExtensionsUnitTests
{
    public class DefaultSqlMetricsTests
    {
        [Theory]
        [InlineData(10, false)]
        [InlineData(50, false)]
        [InlineData(501, true)]
        [InlineData(1001, true)]
        public async Task TestMetrics(int executeMS, bool loggerIt)
        {
            //Arrange
            var autoMocker = new AutoMocker();

            var metrics = autoMocker.CreateInstance<DefaultSqlMetrics>();

            var loggerMock = autoMocker.GetMock<ILogger<DefaultSqlMetrics>>();

            var paras = new {a = 1};
            var Sql = "Sql1";


            metrics.AddToMetrics(Sql, paras);

            await Task.Delay(executeMS);


            metrics.Dispose();

            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Debug,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString().StartsWith("Sql EXECUTE Finished")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Exactly(1));


            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) =>
                        o.ToString()
                            .StartsWith(
                                "Sql EXECUTE Finished")), //string.Equals("Index page say hello", o.ToString(), StringComparison.InvariantCultureIgnoreCase)
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                loggerIt ? Times.Once() : Times.Never());
        }
    }
}
