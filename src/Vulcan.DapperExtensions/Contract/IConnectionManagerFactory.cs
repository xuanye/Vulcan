using System;

namespace Vulcan.DapperExtensions.Contract
{
    public interface IConnectionManagerFactory
    {
        ConnectionManager GetConnectionManager(string constr, IConnectionFactory factory);
    }

}
