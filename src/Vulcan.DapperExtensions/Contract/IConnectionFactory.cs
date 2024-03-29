using System.Data;
using Vulcan.DapperExtensions.ORMapping;

namespace Vulcan.DapperExtensions.Contract
{
    public interface IConnectionFactory
    {
        IDbConnection CreateDbConnection(string connectionString);

        ISQLBuilder SQLBuilder { get; }
    }
}
