using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UUAC.Common;
using UUAC.Interface.Repository;
using Vulcan.DataAccess;

namespace UUAC.DataAccess.Mysql.Repository
{
    public class BaseRepository:Vulcan.DataAccess.ORMapping.MySql.MySqlRepository
    {
        private readonly string _constr;
        private static ILogger<DefaultSQLMetrics> _logger;
        public BaseRepository(IConnectionManagerFactory factory, string constr, ILoggerFactory loggerFactory) : base(factory, constr)
        {
            this._constr = constr;
            if (_logger == null)
            {
                _logger = loggerFactory.CreateLogger<DefaultSQLMetrics>();
            }
        }

        protected override ISQLMetrics CreateSQLMetrics()
        {
            return new DefaultSQLMetrics(_logger);
        }

       
    }
}
