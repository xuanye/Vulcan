using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Vulcan.DapperExtensions.ORMapping.Configuration
{
    /// <summary>
    /// Provides a simple API for configuring an <typeparamref name="T"/> entity type.
    /// </summary>
    /// <typeparam name="T">The entity type being configured.</typeparam>
    public class EntityTypeBuilder<T> where T : BaseEntity
    {
        /// <summary>
        /// Gets or sets the table name for the entity.
        /// </summary>
        internal string TableName { get; private set; }

        /// <summary>
        /// Gets the list of primary key property names.
        /// </summary>
        internal List<string> PrimaryKeyProperties { get; } = new List<string>();

        /// <summary>
        /// Gets the dictionary of property configurations keyed by property name.
        /// </summary>
        internal Dictionary<string, PropertyBuilder> PropertyConfigurations { get; } = new Dictionary<string, PropertyBuilder>();

        /// <summary>
        /// Configures the table name for the entity.
        /// </summary>
        /// <param name="tableName">The name of the table.</param>
        /// <returns>The same builder instance so that multiple configuration calls can be chained.</returns>
        public EntityTypeBuilder<T> ToTable(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentException("Table name cannot be null or empty.", nameof(tableName));
            }

            TableName = tableName;
            return this;
        }

        /// <summary>
        /// Configures a property of the entity.
        /// </summary>
        /// <param name="propertyExpression">A lambda expression representing the property to configure.</param>
        /// <returns>A builder that can be used to configure the property.</returns>
        public PropertyBuilder Property(Expression<Func<T, object>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyExpression));
            }

            string propertyName = GetPropertyName(propertyExpression);

            if (!PropertyConfigurations.TryGetValue(propertyName, out PropertyBuilder propertyBuilder))
            {
                propertyBuilder = new PropertyBuilder(propertyName);
                PropertyConfigurations[propertyName] = propertyBuilder;
            }

            return propertyBuilder;
        }

        /// <summary>
        /// Configures the primary key for the entity.
        /// </summary>
        /// <param name="keyExpression">A lambda expression representing the primary key property or properties.</param>
        /// <returns>The same builder instance so that multiple configuration calls can be chained.</returns>
        public EntityTypeBuilder<T> HasKey(Expression<Func<T, object>> keyExpression)
        {
            if (keyExpression == null)
            {
                throw new ArgumentNullException(nameof(keyExpression));
            }

            string propertyName = GetPropertyName(keyExpression);

            PrimaryKeyProperties.Clear();
            PrimaryKeyProperties.Add(propertyName);

            return this;
        }

        /// <summary>
        /// Extracts the property name from a lambda expression.
        /// </summary>
        /// <param name="expression">The lambda expression.</param>
        /// <returns>The property name.</returns>
        private static string GetPropertyName(Expression<Func<T, object>> expression)
        {
            Expression body = expression.Body;

            // Handle boxing conversion (e.g., x => (object)x.Property)
            if (body is UnaryExpression unaryExpression && unaryExpression.NodeType == ExpressionType.Convert)
            {
                body = unaryExpression.Operand;
            }

            if (body is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }

            throw new ArgumentException("Expression must be a property access expression.", nameof(expression));
        }
    }

    /// <summary>
    /// Provides a simple API for configuring a property.
    /// </summary>
    public class PropertyBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyBuilder"/> class.
        /// </summary>
        /// <param name="propertyName">The name of the property being configured.</param>
        internal PropertyBuilder(string propertyName)
        {
            PropertyName = propertyName;
        }

        /// <summary>
        /// Gets the name of the property being configured.
        /// </summary>
        internal string PropertyName { get; }

        /// <summary>
        /// Gets the configured column name for the property.
        /// </summary>
        internal string ColumnName { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the property is configured as a primary key.
        /// </summary>
        internal bool IsPrimaryKeyConfigured { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the property is configured as an identity/auto-increment column.
        /// </summary>
        internal bool IsIdentityConfigured { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the property is configured as nullable.
        /// </summary>
        internal bool IsNullableConfigured { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the property is configured to be ignored during mapping.
        /// </summary>
        internal bool IsIgnoredConfigured { get; private set; }

        /// <summary>
        /// Configures the column name for the property.
        /// </summary>
        /// <param name="columnName">The name of the column in the database.</param>
        /// <returns>The same builder instance so that multiple configuration calls can be chained.</returns>
        public PropertyBuilder HasColumnName(string columnName)
        {
            if (string.IsNullOrWhiteSpace(columnName))
            {
                throw new ArgumentException("Column name cannot be null or empty.", nameof(columnName));
            }

            ColumnName = columnName;
            return this;
        }

        /// <summary>
        /// Configures the property as a primary key.
        /// </summary>
        /// <returns>The same builder instance so that multiple configuration calls can be chained.</returns>
        public PropertyBuilder IsPrimaryKey()
        {
            IsPrimaryKeyConfigured = true;
            return this;
        }

        /// <summary>
        /// Configures the property as an identity/auto-increment column.
        /// </summary>
        /// <returns>The same builder instance so that multiple configuration calls can be chained.</returns>
        public PropertyBuilder IsIdentity()
        {
            IsIdentityConfigured = true;
            return this;
        }

        /// <summary>
        /// Configures the property as nullable.
        /// </summary>
        /// <returns>The same builder instance so that multiple configuration calls can be chained.</returns>
        public PropertyBuilder IsNullable()
        {
            IsNullableConfigured = true;
            return this;
        }

        /// <summary>
        /// Configures the property to be ignored during mapping.
        /// </summary>
        /// <returns>The same builder instance so that multiple configuration calls can be chained.</returns>
        public PropertyBuilder IsIgnored()
        {
            IsIgnoredConfigured = true;
            return this;
        }
    }
}
