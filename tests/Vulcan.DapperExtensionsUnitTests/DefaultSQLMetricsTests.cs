using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.AutoMock;
using Vulcan.DapperExtensions;
using Xunit;

namespace Vulcan.DapperExtensionsUnitTests
{
    public class DefaultSQLMetricsTests
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

            var metrics = autoMocker.CreateInstance<DefaultSQLMetrics>();

            var loggerMock = autoMocker.GetMock<ILogger<DefaultSQLMetrics>>();

            var paras = new {a = 1};
            var sql = "sql1";


            metrics.AddToMetrics(sql, paras);

            await Task.Delay(executeMS);


            metrics.Dispose();

            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Debug,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString().StartsWith("SQL EXECUTE Finished")),
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
                                "SQL EXECUTE Finished")), //string.Equals("Index page say hello", o.ToString(), StringComparison.InvariantCultureIgnoreCase)
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                loggerIt ? Times.Once() : Times.Never());
        }
    }
}
