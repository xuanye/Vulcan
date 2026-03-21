using System;
using Vulcan.DapperExtensions.ORMapping;
using Vulcan.DapperExtensions.ORMapping.Configuration;
using Xunit;

namespace Vulcan.DapperExtensionsUnitTests.Configuration
{
    /// <summary>
    /// Test entity class for EntityTypeBuilder tests.
    /// </summary>
    public class TestEntity : BaseEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }
    }

    /// <summary>
    /// Unit tests for <see cref="EntityTypeBuilder{T}"/> configuration.
    /// </summary>
    public class EntityTypeBuilderTests
    {
        [Fact]
        public void ToTable_ConfiguresTableName()
        {
            // Arrange
            var builder = new EntityTypeBuilder<TestEntity>();

            // Act
            builder.ToTable("TestTable");

            // Assert
            Assert.Equal("TestTable", builder.TableName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ToTable_WithInvalidTableName_ThrowsArgumentException(string tableName)
        {
            // Arrange
            var builder = new EntityTypeBuilder<TestEntity>();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => builder.ToTable(tableName));
            Assert.Equal("tableName", exception.ParamName);
        }

        [Fact]
        public void Property_ConfiguresColumnName()
        {
            // Arrange
            var builder = new EntityTypeBuilder<TestEntity>();

            // Act
            var propertyBuilder = builder.Property(x => x.Name);

            // Assert
            Assert.NotNull(propertyBuilder);
            Assert.Equal("Name", propertyBuilder.PropertyName);
        }

        [Fact]
        public void Property_ConfiguresColumnName_WithCustomName()
        {
            // Arrange
            var builder = new EntityTypeBuilder<TestEntity>();

            // Act
            var propertyBuilder = builder.Property(x => x.Name).HasColumnName("CustomName");

            // Assert
            Assert.Equal("CustomName", propertyBuilder.ColumnName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Property_HasColumnName_WithInvalidColumnName_ThrowsArgumentException(string columnName)
        {
            // Arrange
            var builder = new EntityTypeBuilder<TestEntity>();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => builder.Property(x => x.Name).HasColumnName(columnName));
            Assert.Equal("columnName", exception.ParamName);
        }

        [Fact]
        public void Property_ConfiguresPrimaryKey()
        {
            // Arrange
            var builder = new EntityTypeBuilder<TestEntity>();

            // Act
            var propertyBuilder = builder.Property(x => x.Id).IsPrimaryKey();

            // Assert
            Assert.True(propertyBuilder.IsPrimaryKeyConfigured);
        }

        [Fact]
        public void Property_ConfiguresIdentity()
        {
            // Arrange
            var builder = new EntityTypeBuilder<TestEntity>();

            // Act
            var propertyBuilder = builder.Property(x => x.Id).IsIdentity();

            // Assert
            Assert.True(propertyBuilder.IsIdentityConfigured);
        }

        [Fact]
        public void Property_ConfiguresNullable()
        {
            // Arrange
            var builder = new EntityTypeBuilder<TestEntity>();

            // Act
            var propertyBuilder = builder.Property(x => x.Description).IsNullable();

            // Assert
            Assert.True(propertyBuilder.IsNullableConfigured);
        }

        [Fact]
        public void Property_ConfiguresIgnored()
        {
            // Arrange
            var builder = new EntityTypeBuilder<TestEntity>();

            // Act
            var propertyBuilder = builder.Property(x => x.IsActive).IsIgnored();

            // Assert
            Assert.True(propertyBuilder.IsIgnoredConfigured);
        }

        [Fact]
        public void HasKey_ConfiguresPrimaryKey()
        {
            // Arrange
            var builder = new EntityTypeBuilder<TestEntity>();

            // Act
            builder.HasKey(x => x.Id);

            // Assert
            Assert.Single(builder.PrimaryKeyProperties);
            Assert.Contains("Id", builder.PrimaryKeyProperties);
        }

        [Fact]
        public void HasKey_WithNullExpression_ThrowsArgumentNullException()
        {
            // Arrange
            var builder = new EntityTypeBuilder<TestEntity>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => builder.HasKey(null));
        }

        [Fact]
        public void Property_WithNullExpression_ThrowsArgumentNullException()
        {
            // Arrange
            var builder = new EntityTypeBuilder<TestEntity>();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => builder.Property(null));
        }

        [Fact]
        public void Property_SupportsChaining()
        {
            // Arrange
            var builder = new EntityTypeBuilder<TestEntity>();

            // Act
            var propertyBuilder = builder.Property(x => x.Id)
                .HasColumnName("CustomId")
                .IsPrimaryKey()
                .IsIdentity()
                .IsNullable();

            // Assert
            Assert.NotNull(propertyBuilder);
            Assert.Equal("Id", propertyBuilder.PropertyName);
            Assert.Equal("CustomId", propertyBuilder.ColumnName);
            Assert.True(propertyBuilder.IsPrimaryKeyConfigured);
            Assert.True(propertyBuilder.IsIdentityConfigured);
            Assert.True(propertyBuilder.IsNullableConfigured);
        }

        [Fact]
        public void EntityTypeBuilder_SupportsChaining()
        {
            // Arrange
            var builder = new EntityTypeBuilder<TestEntity>();

            // Act
            var result = builder.ToTable("TestTable").HasKey(x => x.Id);

            // Assert
            Assert.Same(builder, result);
            Assert.Equal("TestTable", builder.TableName);
            Assert.Single(builder.PrimaryKeyProperties);
            Assert.Contains("Id", builder.PrimaryKeyProperties);
        }

        [Fact]
        public void Property_ReturnsSameBuilderForSameProperty()
        {
            // Arrange
            var builder = new EntityTypeBuilder<TestEntity>();

            // Act
            var propertyBuilder1 = builder.Property(x => x.Name);
            var propertyBuilder2 = builder.Property(x => x.Name);

            // Assert
            Assert.Same(propertyBuilder1, propertyBuilder2);
        }

        [Fact]
        public void Property_StoresConfigurationInDictionary()
        {
            // Arrange
            var builder = new EntityTypeBuilder<TestEntity>();

            // Act
            builder.Property(x => x.Name).HasColumnName("CustomName");
            builder.Property(x => x.Description).IsNullable();

            // Assert
            Assert.Equal(2, builder.PropertyConfigurations.Count);
            Assert.True(builder.PropertyConfigurations.ContainsKey("Name"));
            Assert.True(builder.PropertyConfigurations.ContainsKey("Description"));
            Assert.Equal("CustomName", builder.PropertyConfigurations["Name"].ColumnName);
            Assert.True(builder.PropertyConfigurations["Description"].IsNullableConfigured);
        }

        [Fact]
        public void HasKey_ClearsPreviousPrimaryKey()
        {
            // Arrange
            var builder = new EntityTypeBuilder<TestEntity>();
            builder.HasKey(x => x.Id);

            // Act
            builder.HasKey(x => x.Name);

            // Assert
            Assert.Single(builder.PrimaryKeyProperties);
            Assert.Contains("Name", builder.PrimaryKeyProperties);
            Assert.DoesNotContain("Id", builder.PrimaryKeyProperties);
        }

        [Fact]
        public void Property_DefaultConfigurationValues()
        {
            // Arrange
            var builder = new EntityTypeBuilder<TestEntity>();

            // Act
            var propertyBuilder = builder.Property(x => x.Name);

            // Assert
            Assert.Null(propertyBuilder.ColumnName);
            Assert.False(propertyBuilder.IsPrimaryKeyConfigured);
            Assert.False(propertyBuilder.IsIdentityConfigured);
            Assert.False(propertyBuilder.IsNullableConfigured);
            Assert.False(propertyBuilder.IsIgnoredConfigured);
        }
    }
}
