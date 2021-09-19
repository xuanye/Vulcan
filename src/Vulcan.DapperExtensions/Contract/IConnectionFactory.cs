using System.Data;

namespace Vulcan.DapperExtensions.Contract
{
    public interface IConnectionFactory
    {
        IDbConnection CreateDbConnection(string connectionString);
    }
}
