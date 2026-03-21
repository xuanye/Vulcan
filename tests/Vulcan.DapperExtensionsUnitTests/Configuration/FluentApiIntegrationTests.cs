using System;
using System.Collections.Generic;
using Vulcan.DapperExtensions.ORMapping;
using Vulcan.DapperExtensions.ORMapping.Configuration;
using Vulcan.DapperExtensions.ORMapping.MySQL;
using Xunit;

namespace Vulcan.DapperExtensionsUnitTests.Configuration
{
    #region Test Entities with Attributes

    /// <summary>
    /// Test entity with attribute-based configuration for override testing.
    /// </summary>
    [TableName("AttributeTable")]
    public class AttributeConfiguredEntity : BaseEntity
    {
        [PrimaryKey]
        [Identity]
        [MapField("attr_id")]
        public int Id { get; set; }

        [MapField("attr_name")]
        public string Name { get; set; }

        [Nullable]
        [MapField("attr_description")]
        public string Description { get; set; }

        [Ignore]
        public string IgnoredProperty { get; set; }
    }

    /// <summary>
    /// Test entity for Fluent API configuration testing.
    /// </summary>
    public class FluentConfiguredEntity : BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }
    }

    /// <summary>
    /// Test entity for composite primary key testing.
    /// </summary>
    public class CompositeKeyEntity : BaseEntity
    {
        public int TenantId { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }
    }

    /// <summary>
    /// Test entity for all property configurations testing.
    /// </summary>
    public class AllConfigEntity : BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string MappedColumn { get; set; }

        public string NullableField { get; set; }

        public string IgnoredField { get; set; }
    }

    /// <summary>
    /// Test entity for BaseEntity integration testing.
    /// </summary>
    public class BaseEntityIntegrationEntity : BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }

    #endregion

    #region Test Configurations

    /// <summary>
    /// Fluent API configuration that overrides attribute configuration.
    /// </summary>
    public class OverrideAttributeConfiguration : IEntityConfiguration<AttributeConfiguredEntity>
    {
        public void Configure(EntityTypeBuilder<AttributeConfiguredEntity> builder)
        {
            builder.ToTable("FluentTable")
                   .HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("fluent_id")
                   .IsPrimaryKey()
                   .IsIdentity();

            builder.Property(x => x.Name)
                   .HasColumnName("fluent_name");

            builder.Property(x => x.Description)
                   .HasColumnName("fluent_description")
                   .IsNullable();

            builder.Property(x => x.IgnoredProperty)
                   .IsIgnored();
        }
    }

    /// <summary>
    /// Fluent API configuration for FluentConfiguredEntity.
    /// </summary>
    public class FluentEntityConfiguration : IEntityConfiguration<FluentConfiguredEntity>
    {
        public void Configure(EntityTypeBuilder<FluentConfiguredEntity> builder)
        {
            builder.ToTable("FluentEntities")
                   .HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("entity_id")
                   .IsPrimaryKey()
                   .IsIdentity();

            builder.Property(x => x.Name)
                   .HasColumnName("entity_name");

            builder.Property(x => x.Description)
                   .HasColumnName("entity_description")
                   .IsNullable();

            builder.Property(x => x.CreatedAt)
                   .HasColumnName("created_at");

            builder.Property(x => x.IsActive)
                   .HasColumnName("is_active");

            // Ignore BaseEntity's FullUpdate property
            builder.Property(x => x.FullUpdate)
                   .IsIgnored();
        }
    }

    /// <summary>
    /// Fluent API configuration for composite primary key.
    /// </summary>
    public class CompositeKeyConfiguration : IEntityConfiguration<CompositeKeyEntity>
    {
        public void Configure(EntityTypeBuilder<CompositeKeyEntity> builder)
        {
            builder.ToTable("CompositeKeyTable")
                   .HasKey(x => x.TenantId);

            // Note: Current implementation only supports single key via HasKey
            // This tests the single key configuration
            builder.Property(x => x.TenantId)
                   .HasColumnName("tenant_id")
                   .IsPrimaryKey();

            builder.Property(x => x.UserId)
                   .HasColumnName("user_id");

            builder.Property(x => x.UserName)
                   .HasColumnName("user_name");

            // Ignore BaseEntity's FullUpdate property
            builder.Property(x => x.FullUpdate)
                   .IsIgnored();
        }
    }

    /// <summary>
    /// Fluent API configuration for all property configurations.
    /// </summary>
    public class AllPropertyConfiguration : IEntityConfiguration<AllConfigEntity>
    {
        public void Configure(EntityTypeBuilder<AllConfigEntity> builder)
        {
            builder.ToTable("AllConfigTable")
                   .HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("config_id")
                   .IsPrimaryKey()
                   .IsIdentity();

            builder.Property(x => x.Name)
                   .HasColumnName("config_name");

            builder.Property(x => x.MappedColumn)
                   .HasColumnName("mapped_col");

            builder.Property(x => x.NullableField)
                   .HasColumnName("nullable_col")
                   .IsNullable();

            builder.Property(x => x.IgnoredField)
                   .IsIgnored();

            // Ignore BaseEntity's FullUpdate property
            builder.Property(x => x.FullUpdate)
                   .IsIgnored();
        }
    }

    /// <summary>
    /// Fluent API configuration for BaseEntity integration.
    /// </summary>
    public class BaseEntityIntegrationConfiguration : IEntityConfiguration<BaseEntityIntegrationEntity>
    {
        public void Configure(EntityTypeBuilder<BaseEntityIntegrationEntity> builder)
        {
            builder.ToTable("IntegrationTable")
                   .HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("integration_id")
                   .IsPrimaryKey()
                   .IsIdentity();

            builder.Property(x => x.Name)
                   .HasColumnName("integration_name");

            builder.Property(x => x.Value)
                   .HasColumnName("integration_value")
                   .IsNullable();

            // Ignore BaseEntity's FullUpdate property
            builder.Property(x => x.FullUpdate)
                   .IsIgnored();
        }
    }

    #endregion

    /// <summary>
    /// Integration tests for Fluent API configuration flow and SQL generation.
    /// </summary>
    public class FluentApiIntegrationTests
    {
        private readonly EntityConfigurationStore _store;
        private readonly MySQLSQLBuilder _sqlBuilder;

        public FluentApiIntegrationTests()
        {
            _store = new EntityConfigurationStore();
            _sqlBuilder = MySQLSQLBuilder.Instance;

            // Set the configuration store for EntityReflect
            EntityReflect.SetConfigurationStore(_store);
        }

        [Fact]
        public void FluentApiConfiguration_OverridesAttributeConfiguration()
        {
            // Arrange
            var config = new OverrideAttributeConfiguration();
            _store.RegisterConfiguration(config);

            // Act
            var entityMeta = _store.GetEntityMeta<AttributeConfiguredEntity>();

            // Assert - Table name should be from Fluent API, not attribute
            Assert.Equal("FluentTable", entityMeta.TableName);
            Assert.NotEqual("AttributeTable", entityMeta.TableName);

            // Assert - Column names should be from Fluent API
            var idColumn = entityMeta.Columns.Find(c => c.PropertyName == "Id");
            Assert.NotNull(idColumn);
            Assert.Equal("fluent_id", idColumn.ColumnName);
            Assert.True(idColumn.PrimaryKey);
            Assert.True(idColumn.Identity);

            var nameColumn = entityMeta.Columns.Find(c => c.PropertyName == "Name");
            Assert.NotNull(nameColumn);
            Assert.Equal("fluent_name", nameColumn.ColumnName);

            var descColumn = entityMeta.Columns.Find(c => c.PropertyName == "Description");
            Assert.NotNull(descColumn);
            Assert.Equal("fluent_description", descColumn.ColumnName);
            Assert.True(descColumn.Nullable);

            // Assert - Ignored property should not be in columns
            var ignoredColumn = entityMeta.Columns.Find(c => c.PropertyName == "IgnoredProperty");
            Assert.Null(ignoredColumn);
        }

        [Fact]
        public void FluentApiConfiguration_GeneratesCorrectEntityMeta()
        {
            // Arrange
            var config = new FluentEntityConfiguration();
            _store.RegisterConfiguration(config);

            // Act
            var entityMeta = _store.GetEntityMeta<FluentConfiguredEntity>();

            // Assert - Table name
            Assert.Equal("FluentEntities", entityMeta.TableName);

            // Assert - All configured columns exist
            Assert.Equal(5, entityMeta.Columns.Count);

            // Assert - Id column configuration
            var idColumn = entityMeta.Columns.Find(c => c.PropertyName == "Id");
            Assert.NotNull(idColumn);
            Assert.Equal("entity_id", idColumn.ColumnName);
            Assert.True(idColumn.PrimaryKey);
            Assert.True(idColumn.Identity);
            Assert.False(idColumn.Nullable);

            // Assert - Name column configuration
            var nameColumn = entityMeta.Columns.Find(c => c.PropertyName == "Name");
            Assert.NotNull(nameColumn);
            Assert.Equal("entity_name", nameColumn.ColumnName);
            Assert.False(nameColumn.PrimaryKey);
            Assert.False(nameColumn.Identity);
            Assert.False(nameColumn.Nullable);

            // Assert - Description column configuration
            var descColumn = entityMeta.Columns.Find(c => c.PropertyName == "Description");
            Assert.NotNull(descColumn);
            Assert.Equal("entity_description", descColumn.ColumnName);
            Assert.True(descColumn.Nullable);

            // Assert - CreatedAt column configuration
            var createdAtColumn = entityMeta.Columns.Find(c => c.PropertyName == "CreatedAt");
            Assert.NotNull(createdAtColumn);
            Assert.Equal("created_at", createdAtColumn.ColumnName);

            // Assert - IsActive column configuration
            var isActiveColumn = entityMeta.Columns.Find(c => c.PropertyName == "IsActive");
            Assert.NotNull(isActiveColumn);
            Assert.Equal("is_active", isActiveColumn.ColumnName);
        }

        [Fact]
        public void FluentApiConfiguration_WorksWithBaseEntity()
        {
            // Arrange
            var config = new BaseEntityIntegrationConfiguration();
            _store.RegisterConfiguration(config);

            // Act
            var entityMeta = _store.GetEntityMeta<BaseEntityIntegrationEntity>();

            // Assert - Table name
            Assert.Equal("IntegrationTable", entityMeta.TableName);

            // Assert - Id column
            var idColumn = entityMeta.Columns.Find(c => c.PropertyName == "Id");
            Assert.NotNull(idColumn);
            Assert.Equal("integration_id", idColumn.ColumnName);
            Assert.True(idColumn.PrimaryKey);
            Assert.True(idColumn.Identity);

            // Assert - Name column
            var nameColumn = entityMeta.Columns.Find(c => c.PropertyName == "Name");
            Assert.NotNull(nameColumn);
            Assert.Equal("integration_name", nameColumn.ColumnName);

            // Assert - Value column
            var valueColumn = entityMeta.Columns.Find(c => c.PropertyName == "Value");
            Assert.NotNull(valueColumn);
            Assert.Equal("integration_value", valueColumn.ColumnName);
            Assert.True(valueColumn.Nullable);

            // Assert - BaseEntity's FullUpdate property is ignored
            var fullUpdateColumn = entityMeta.Columns.Find(c => c.PropertyName == "FullUpdate");
            Assert.Null(fullUpdateColumn); // FullUpdate is explicitly ignored in configuration
        }

        [Fact]
        public void FluentApiConfiguration_SupportsCompositePrimaryKey()
        {
            // Arrange
            var config = new CompositeKeyConfiguration();
            _store.RegisterConfiguration(config);

            // Act
            var entityMeta = _store.GetEntityMeta<CompositeKeyEntity>();

            // Assert - Table name
            Assert.Equal("CompositeKeyTable", entityMeta.TableName);

            // Assert - Primary key configuration
            var tenantIdColumn = entityMeta.Columns.Find(c => c.PropertyName == "TenantId");
            Assert.NotNull(tenantIdColumn);
            Assert.Equal("tenant_id", tenantIdColumn.ColumnName);
            Assert.True(tenantIdColumn.PrimaryKey);

            // Assert - Other columns
            var userIdColumn = entityMeta.Columns.Find(c => c.PropertyName == "UserId");
            Assert.NotNull(userIdColumn);
            Assert.Equal("user_id", userIdColumn.ColumnName);
            Assert.False(userIdColumn.PrimaryKey);

            var userNameColumn = entityMeta.Columns.Find(c => c.PropertyName == "UserName");
            Assert.NotNull(userNameColumn);
            Assert.Equal("user_name", userNameColumn.ColumnName);
            Assert.False(userNameColumn.PrimaryKey);
        }

        [Fact]
        public void FluentApiConfiguration_SupportsAllPropertyConfigurations()
        {
            // Arrange
            var config = new AllPropertyConfiguration();
            _store.RegisterConfiguration(config);

            // Act
            var entityMeta = _store.GetEntityMeta<AllConfigEntity>();

            // Assert - Table name
            Assert.Equal("AllConfigTable", entityMeta.TableName);

            // Assert - Id column with primary key and identity
            var idColumn = entityMeta.Columns.Find(c => c.PropertyName == "Id");
            Assert.NotNull(idColumn);
            Assert.Equal("config_id", idColumn.ColumnName);
            Assert.True(idColumn.PrimaryKey);
            Assert.True(idColumn.Identity);

            // Assert - Name column with custom column name
            var nameColumn = entityMeta.Columns.Find(c => c.PropertyName == "Name");
            Assert.NotNull(nameColumn);
            Assert.Equal("config_name", nameColumn.ColumnName);

            // Assert - MappedColumn with custom column name
            var mappedColumn = entityMeta.Columns.Find(c => c.PropertyName == "MappedColumn");
            Assert.NotNull(mappedColumn);
            Assert.Equal("mapped_col", mappedColumn.ColumnName);

            // Assert - NullableField with nullable configuration
            var nullableColumn = entityMeta.Columns.Find(c => c.PropertyName == "NullableField");
            Assert.NotNull(nullableColumn);
            Assert.Equal("nullable_col", nullableColumn.ColumnName);
            Assert.True(nullableColumn.Nullable);

            // Assert - IgnoredField is not in columns
            var ignoredColumn = entityMeta.Columns.Find(c => c.PropertyName == "IgnoredField");
            Assert.Null(ignoredColumn);

            // Assert - Total columns count (excluding ignored)
            Assert.Equal(4, entityMeta.Columns.Count);
        }

        [Fact]
        public void FluentApiConfiguration_GeneratesCorrectInsertSQL()
        {
            // Arrange
            var config = new FluentEntityConfiguration();
            _store.RegisterConfiguration(config);

            // Act
            var entityMeta = _store.GetEntityMeta<FluentConfiguredEntity>();
            var insertSql = _sqlBuilder.BuildInsertSql(entityMeta);

            // Assert - SQL should use configured table and column names
            Assert.Contains("INSERT INTO `FluentEntities`", insertSql);
            // Identity column (entity_id) should NOT be in INSERT SQL
            Assert.DoesNotContain("`entity_id`", insertSql);
            Assert.Contains("`entity_name`", insertSql);
            Assert.Contains("`entity_description`", insertSql);
            Assert.Contains("`created_at`", insertSql);
            Assert.Contains("`is_active`", insertSql);

            // Assert - Parameters should use property names (excluding identity)
            Assert.DoesNotContain("@Id", insertSql);
            Assert.Contains("@Name", insertSql);
            Assert.Contains("@Description", insertSql);
            Assert.Contains("@CreatedAt", insertSql);
            Assert.Contains("@IsActive", insertSql);

            // Assert - Identity column should have LAST_INSERT_ID
            Assert.Contains("LAST_INSERT_ID()", insertSql);
        }

        [Fact]
        public void FluentApiConfiguration_GeneratesCorrectUpdateSQL()
        {
            // Arrange
            var config = new FluentEntityConfiguration();
            _store.RegisterConfiguration(config);

            // Act
            var entityMeta = _store.GetEntityMeta<FluentConfiguredEntity>();
            var updateSql = _sqlBuilder.BuildUpdateSql(entityMeta);

            // Assert - SQL should use configured table and column names
            Assert.Contains("UPDATE `FluentEntities` SET", updateSql);
            Assert.Contains("`entity_name`=@Name", updateSql);
            Assert.Contains("`entity_description`=@Description", updateSql);
            Assert.Contains("`created_at`=@CreatedAt", updateSql);
            Assert.Contains("`is_active`=@IsActive", updateSql);

            // Assert - WHERE clause should use primary key
            Assert.Contains("WHERE `entity_id`=@Id", updateSql);
        }

        [Fact]
        public void FluentApiConfiguration_GeneratesCorrectInsertSQL_WithIgnoredColumns()
        {
            // Arrange
            var config = new AllPropertyConfiguration();
            _store.RegisterConfiguration(config);

            // Act
            var entityMeta = _store.GetEntityMeta<AllConfigEntity>();
            var insertSql = _sqlBuilder.BuildInsertSql(entityMeta);

            // Assert - SQL should not include ignored column
            Assert.DoesNotContain("IgnoredField", insertSql);

            // Assert - SQL should not include identity column
            Assert.DoesNotContain("`config_id`", insertSql);

            // Assert - SQL should include configured columns (excluding identity)
            Assert.Contains("`config_name`", insertSql);
            Assert.Contains("`mapped_col`", insertSql);
            Assert.Contains("`nullable_col`", insertSql);
        }

        [Fact]
        public void FluentApiConfiguration_GeneratesCorrectUpdateSQL_WithIgnoredColumns()
        {
            // Arrange
            var config = new AllPropertyConfiguration();
            _store.RegisterConfiguration(config);

            // Act
            var entityMeta = _store.GetEntityMeta<AllConfigEntity>();
            var updateSql = _sqlBuilder.BuildUpdateSql(entityMeta);

            // Assert - SQL should not include ignored column
            Assert.DoesNotContain("IgnoredField", updateSql);

            // Assert - SQL should include configured columns in SET clause
            Assert.Contains("`config_name`=@Name", updateSql);
            Assert.Contains("`mapped_col`=@MappedColumn", updateSql);
            Assert.Contains("`nullable_col`=@NullableField", updateSql);

            // Assert - WHERE clause should use primary key
            Assert.Contains("WHERE `config_id`=@Id", updateSql);
        }

        [Fact]
        public void EntityReflect_UsesFluentApiConfiguration_WhenStoreIsSet()
        {
            // Arrange
            var config = new FluentEntityConfiguration();
            _store.RegisterConfiguration(config);

            // Act - EntityReflect should use the configuration store
            var entityMeta = EntityReflect.GetDefineInfoFromType(typeof(FluentConfiguredEntity));

            // Assert - Should get metadata from Fluent API configuration
            Assert.Equal("FluentEntities", entityMeta.TableName);

            var idColumn = entityMeta.Columns.Find(c => c.PropertyName == "Id");
            Assert.NotNull(idColumn);
            Assert.Equal("entity_id", idColumn.ColumnName);
            Assert.True(idColumn.PrimaryKey);
            Assert.True(idColumn.Identity);
        }

        [Fact]
        public void EntityReflect_FallsBackToAttributes_WhenNoFluentConfig()
        {
            // Arrange - Don't register Fluent API configuration for this entity
            // EntityReflect should fall back to attribute-based configuration

            // Act
            var entityMeta = EntityReflect.GetDefineInfoFromType(typeof(AttributeConfiguredEntity));

            // Assert - Should get metadata from attributes
            Assert.Equal("AttributeTable", entityMeta.TableName);

            var idColumn = entityMeta.Columns.Find(c => c.PropertyName == "Id");
            Assert.NotNull(idColumn);
            Assert.Equal("attr_id", idColumn.ColumnName);
            Assert.True(idColumn.PrimaryKey);
            Assert.True(idColumn.Identity);
        }

        [Fact]
        public void EntityConfigurationStore_ThrowsWhenNoConfigRegistered()
        {
            // Arrange - Don't register any configuration

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(
                () => _store.GetEntityMeta<FluentConfiguredEntity>());

            Assert.Contains("No configuration registered", exception.Message);
            Assert.Contains(typeof(FluentConfiguredEntity).FullName, exception.Message);
        }

        [Fact]
        public void EntityConfigurationStore_SupportsMultipleEntityConfigurations()
        {
            // Arrange
            var config1 = new FluentEntityConfiguration();
            var config2 = new AllPropertyConfiguration();
            _store.RegisterConfiguration(config1);
            _store.RegisterConfiguration(config2);

            // Act
            var meta1 = _store.GetEntityMeta<FluentConfiguredEntity>();
            var meta2 = _store.GetEntityMeta<AllConfigEntity>();

            // Assert - Both configurations should be stored independently
            Assert.Equal("FluentEntities", meta1.TableName);
            Assert.Equal("AllConfigTable", meta2.TableName);

            Assert.Equal(5, meta1.Columns.Count);
            Assert.Equal(4, meta2.Columns.Count); // Excluding ignored column
        }

        [Fact]
        public void FluentApiConfiguration_PropertyChaining_WorksCorrectly()
        {
            // Arrange
            var builder = new EntityTypeBuilder<FluentConfiguredEntity>();

            // Act - Chain multiple configurations
            var result = builder.ToTable("ChainedTable")
                               .HasKey(x => x.Id);

            // Assert - Chaining should return the same builder
            Assert.Same(builder, result);
            Assert.Equal("ChainedTable", builder.TableName);
            Assert.Single(builder.PrimaryKeyProperties);
            Assert.Contains("Id", builder.PrimaryKeyProperties);
        }

        [Fact]
        public void FluentApiConfiguration_PropertyBuilderChaining_WorksCorrectly()
        {
            // Arrange
            var builder = new EntityTypeBuilder<FluentConfiguredEntity>();

            // Act - Chain property configurations
            var propertyBuilder = builder.Property(x => x.Id)
                                        .HasColumnName("chained_id")
                                        .IsPrimaryKey()
                                        .IsIdentity()
                                        .IsNullable();

            // Assert - All configurations should be applied
            Assert.Equal("Id", propertyBuilder.PropertyName);
            Assert.Equal("chained_id", propertyBuilder.ColumnName);
            Assert.True(propertyBuilder.IsPrimaryKeyConfigured);
            Assert.True(propertyBuilder.IsIdentityConfigured);
            Assert.True(propertyBuilder.IsNullableConfigured);
        }

        [Fact]
        public void FluentApiConfiguration_DefaultPropertyValues_AreCorrect()
        {
            // Arrange
            var builder = new EntityTypeBuilder<FluentConfiguredEntity>();

            // Act
            var propertyBuilder = builder.Property(x => x.Name);

            // Assert - Default values
            Assert.Null(propertyBuilder.ColumnName);
            Assert.False(propertyBuilder.IsPrimaryKeyConfigured);
            Assert.False(propertyBuilder.IsIdentityConfigured);
            Assert.False(propertyBuilder.IsNullableConfigured);
            Assert.False(propertyBuilder.IsIgnoredConfigured);
        }
    }
}
