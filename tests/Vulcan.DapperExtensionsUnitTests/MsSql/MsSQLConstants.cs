using System;
using System.Collections.Generic;
using System.Text;

namespace Vulcan.DapperExtensionsUnitTests.MsSql
{
    public static class MSSQLConstants
    {
        /// <summary>
        /// MsSQL Local TestDb
        /// NOTE:TestDb Should be exists;
        /// </summary>
        public const string CONNECTION_STRING = @"Server=(LocalDB)\MSSQLLocalDB;Initial Catalog=TestDb;Integrated Security=true;";
    }
}
