using System;

namespace Vulcan.DapperExtensions.ORMapping
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableNameAttribute : Attribute
    {
        public TableNameAttribute(string tableName)
        {
            TableName = tableName;
        }

        public string TableName { get; }
    }
}
