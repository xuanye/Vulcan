using System;

namespace Vulcan.DapperExtensions.ORMapping
{
    [AttributeUsage(AttributeTargets.Property)]
    public class MapFieldAttribute : Attribute
    {
        public MapFieldAttribute(string mapField)
        {
            this.MapFieldName = mapField;
        }

        public string MapFieldName { get; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class NullableAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class IdentityAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKeyAttribute : Attribute
    {
        public PrimaryKeyAttribute()
            : this(0)
        {
        }

        public PrimaryKeyAttribute(int pkIndex)
        {
            this.PKIndex = pkIndex;
        }

        public int PKIndex { get; }
    }

    public class IgnoreAttribute : Attribute
    {
    }
}
