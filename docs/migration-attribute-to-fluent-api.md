# Vulcan ORM 迁移指南：从 Attribute 到 Fluent API

> **文档版本**: 1.0  
> **最后更新**: 2026-03-21  
> **适用版本**: Vulcan.DapperExtensions 2.x+

---

## 📋 目录

- [1. 概述](#1-概述)
- [2. 为什么迁移](#2-为什么迁移)
- [3. 迁移前准备](#3-迁移前准备)
- [4. 迁移步骤](#4-迁移步骤)
- [5. 配置对照表](#5-配置对照表)
- [6. 完整迁移示例](#6-完整迁移示例)
- [7. 高级用法](#7-高级用法)
- [8. 注意事项](#8-注意事项)
- [9. 常见问题](#9-常见问题)
- [10. 回滚方案](#10-回滚方案)

---

## 1. 概述

Vulcan ORM 提供了两种实体映射配置方式：

| 配置方式 | 特点 | 适用场景 |
|---------|------|---------|
| **Attribute** | 简单直观、代码量少 | 快速开发、小型项目 |
| **Fluent API** | 类型安全、配置分离 | 大型项目、需要重构 |

本文档指导您将现有的 Attribute 配置平滑迁移到 Fluent API 方式。

---

## 2. 为什么迁移

### 2.1 Fluent API 的优势

```
┌─────────────────────────────────────────────────────────────┐
│                    Fluent API 优势对比                       │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  ✅ 类型安全          编译时检查，避免运行时错误              │
│  ✅ 配置分离          实体类与映射配置解耦                    │
│  ✅ 重构友好          IDE 自动重命名支持                      │
│  ✅ 复杂逻辑          支持条件配置、动态配置                  │
│  ✅ 可读性强          链式调用，语义清晰                      │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

### 2.2 适用迁移场景

- ✅ 项目规模较大，需要更好的代码组织
- ✅ 频繁重构，需要 IDE 支持
- ✅ 需要在运行时动态配置映射
- ✅ 希望实体类保持"干净"（POCO）
- ✅ 团队开发，需要统一配置管理

---

## 3. 迁移前准备

### 3.1 环境要求

```xml
<!-- 确保使用 Vulcan.DapperExtensions 2.x 或更高版本 -->
<PackageReference Include="Vulcan.DapperExtensions" Version="2.*" />
```

### 3.2 项目结构建议

```
YourProject/
├── Entities/
│   ├── UserEntity.cs
│   ├── OrderEntity.cs
│   └── ProductEntity.cs
├── Configurations/           # 新增：Fluent API 配置目录
│   ├── UserEntityConfiguration.cs
│   ├── OrderEntityConfiguration.cs
│   └── ProductEntityConfiguration.cs
└── Startup.cs                # 注册配置
```

### 3.3 备份现有代码

```bash
# 创建备份分支
git checkout -b backup/attribute-mapping
git push origin backup/attribute-mapping

# 或者创建备份目录
mkdir -p backup/entities
cp Entities/*.cs backup/entities/
```

---

## 4. 迁移步骤

### 步骤 1：创建配置类

为每个实体创建对应的配置类：

```csharp
using Vulcan.DapperExtensions.ORMapping;
using Vulcan.DapperExtensions.ORMapping.Configuration;

namespace YourProject.Configurations
{
    public class UserEntityConfiguration : EntityTypeConfiguration<UserEntity>
    {
        public override void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            // 配置将在这里添加
        }
    }
}
```

### 步骤 2：迁移表名配置

**Attribute 方式：**
```csharp
[TableName("user_info")]
public class UserEntity : BaseEntity
{
    // ...
}
```

**Fluent API 方式：**
```csharp
public class UserEntityConfiguration : EntityTypeConfiguration<UserEntity>
{
    public override void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("user_info");
    }
}
```

### 步骤 3：迁移主键配置

**Attribute 方式：**
```csharp
[PrimaryKey]
public int Id { get; set; }
```

**Fluent API 方式：**
```csharp
builder.HasKey(x => x.Id);
```

### 步骤 4：迁移列映射配置

**Attribute 方式：**
```csharp
[MapField("user_name")]
public string UserName { get; set; }
```

**Fluent API 方式：**
```csharp
builder.Property(x => x.UserName)
       .HasColumnName("user_name");
```

### 步骤 5：迁移其他属性配置

**Attribute 方式：**
```csharp
[Identity]
public int Id { get; set; }

[Nullable]
public string Email { get; set; }

[Ignore]
public string TempField { get; set; }
```

**Fluent API 方式：**
```csharp
builder.Property(x => x.Id).IsIdentity();
builder.Property(x => x.Email).IsNullable();
builder.Property(x => x.TempField).IsIgnored();
```

### 步骤 6：注册配置

**在应用启动时注册（ASP.NET Core）：**

```csharp
// Startup.cs 或 Program.cs
public void ConfigureServices(IServiceCollection services)
{
    // 创建配置存储
    var configStore = new EntityConfigurationStore();
    
    // 注册所有实体配置
    configStore.RegisterConfiguration(new UserEntityConfiguration());
    configStore.RegisterConfiguration(new OrderEntityConfiguration());
    configStore.RegisterConfiguration(new ProductEntityConfiguration());
    
    // 设置到 EntityReflect
    EntityReflect.SetConfigurationStore(configStore);
    
    // 其他服务配置...
}
```

**批量注册（推荐）：**

```csharp
public static class EntityConfigurationExtensions
{
    public static void RegisterAllConfigurations(
        this EntityConfigurationStore store, 
        Assembly assembly)
    {
        var configTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && 
                   t.BaseType?.IsGenericType == true &&
                   t.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
        
        foreach (var configType in configTypes)
        {
            var configInstance = Activator.CreateInstance(configType);
            var entityType = configType.BaseType.GetGenericArguments()[0];
            
            var method = typeof(EntityConfigurationStore)
                .GetMethod(nameof(EntityConfigurationStore.RegisterConfiguration))
                .MakeGenericMethod(entityType);
            
            method.Invoke(store, new[] { configInstance });
        }
    }
}

// 使用方式
var configStore = new EntityConfigurationStore();
configStore.RegisterAllConfigurations(typeof(Startup).Assembly);
EntityReflect.SetConfigurationStore(configStore);
```

### 步骤 7：移除 Attribute

确认 Fluent API 配置生效后，移除实体类上的 Attribute：

```csharp
// 迁移前
[TableName("user_info")]
public class UserEntity : BaseEntity
{
    [PrimaryKey]
    [Identity]
    [MapField("id")]
    public int Id { get; set; }
    
    [MapField("user_name")]
    public string UserName { get; set; }
}

// 迁移后（干净的 POCO）
public class UserEntity : BaseEntity
{
    public int Id { get; set; }
    public string UserName { get; set; }
}
```

---

## 5. 配置对照表

### 5.1 基础配置对照

| Attribute | Fluent API | 说明 |
|-----------|-----------|------|
| `[TableName("xxx")]` | `builder.ToTable("xxx")` | 配置表名 |
| `[PrimaryKey]` | `builder.HasKey(x => x.Xxx)` | 配置主键 |
| `[MapField("xxx")]` | `builder.Property(x => x.Xxx).HasColumnName("xxx")` | 列名映射 |
| `[Identity]` | `builder.Property(x => x.Xxx).IsIdentity()` | 自增列 |
| `[Nullable]` | `builder.Property(x => x.Xxx).IsNullable()` | 可空列 |
| `[Ignore]` | `builder.Property(x => x.Xxx).IsIgnored()` | 忽略字段 |

### 5.2 组合配置对照

**Attribute 方式（组合使用）：**
```csharp
[MapField("id")]
[PrimaryKey]
[Identity]
public int Id { get; set; }
```

**Fluent API 方式（链式调用）：**
```csharp
builder.Property(x => x.Id)
       .HasColumnName("id")
       .IsIdentity();
// 主键单独配置
builder.HasKey(x => x.Id);
```

### 5.3 完整配置对照

| 场景 | Attribute | Fluent API |
|------|-----------|-----------|
| 表名 | `[TableName("users")]` | `.ToTable("users")` |
| 主键 | `[PrimaryKey]` | `.HasKey(x => x.Id)` |
| 自增主键 | `[PrimaryKey][Identity]` | `.HasKey(x => x.Id)` + `.Property(x => x.Id).IsIdentity()` |
| 列映射 | `[MapField("user_name")]` | `.Property(x => x.UserName).HasColumnName("user_name")` |
| 可空 | `[Nullable]` | `.Property(x => x.Email).IsNullable()` |
| 忽略 | `[Ignore]` | `.Property(x => x.Temp).IsIgnored()` |
| 复合主键 | 不支持 | `.HasKey(x => new { x.TenantId, x.UserId })` |

---

## 6. 完整迁移示例

### 6.1 迁移前（Attribute 方式）

**实体类：**
```csharp
using Vulcan.DapperExtensions.ORMapping;

namespace YourProject.Entities
{
    [TableName("user_info")]
    public class UserEntity : BaseEntity
    {
        private int _Id;
        private string _UserName;
        private string _Email;
        private string _Phone;
        private DateTime _CreateTime;
        private string _TempField;

        [MapField("id")]
        [PrimaryKey]
        [Identity]
        public int Id
        {
            get => _Id;
            set
            {
                _Id = value;
                OnPropertyChanged("id");
            }
        }

        [MapField("user_name")]
        public string UserName
        {
            get => _UserName;
            set
            {
                _UserName = value;
                OnPropertyChanged("user_name");
            }
        }

        [MapField("email")]
        [Nullable]
        public string Email
        {
            get => _Email;
            set
            {
                _Email = value;
                OnPropertyChanged("email");
            }
        }

        [MapField("phone")]
        [Nullable]
        public string Phone
        {
            get => _Phone;
            set
            {
                _Phone = value;
                OnPropertyChanged("phone");
            }
        }

        [MapField("create_time")]
        public DateTime CreateTime
        {
            get => _CreateTime;
            set
            {
                _CreateTime = value;
                OnPropertyChanged("create_time");
            }
        }

        [Ignore]
        public string TempField
        {
            get => _TempField;
            set => _TempField = value;
        }
    }
}
```

### 6.2 迁移后（Fluent API 方式）

**实体类（简化）：**
```csharp
using Vulcan.DapperExtensions.ORMapping;

namespace YourProject.Entities
{
    public class UserEntity : BaseEntity
    {
        private int _Id;
        private string _UserName;
        private string _Email;
        private string _Phone;
        private DateTime _CreateTime;
        private string _TempField;

        public int Id
        {
            get => _Id;
            set
            {
                _Id = value;
                OnPropertyChanged("id");
            }
        }

        public string UserName
        {
            get => _UserName;
            set
            {
                _UserName = value;
                OnPropertyChanged("user_name");
            }
        }

        public string Email
        {
            get => _Email;
            set
            {
                _Email = value;
                OnPropertyChanged("email");
            }
        }

        public string Phone
        {
            get => _Phone;
            set
            {
                _Phone = value;
                OnPropertyChanged("phone");
            }
        }

        public DateTime CreateTime
        {
            get => _CreateTime;
            set
            {
                _CreateTime = value;
                OnPropertyChanged("create_time");
            }
        }

        public string TempField
        {
            get => _TempField;
            set => _TempField = value;
        }
    }
}
```

**配置类（新建）：**
```csharp
using Vulcan.DapperExtensions.ORMapping;
using Vulcan.DapperExtensions.ORMapping.Configuration;

namespace YourProject.Configurations
{
    public class UserEntityConfiguration : EntityTypeConfiguration<UserEntity>
    {
        public override void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            // 配置表名
            builder.ToTable("user_info");

            // 配置主键
            builder.HasKey(x => x.Id);

            // 配置各列
            builder.Property(x => x.Id)
                   .HasColumnName("id")
                   .IsIdentity();

            builder.Property(x => x.UserName)
                   .HasColumnName("user_name");

            builder.Property(x => x.Email)
                   .HasColumnName("email")
                   .IsNullable();

            builder.Property(x => x.Phone)
                   .HasColumnName("phone")
                   .IsNullable();

            builder.Property(x => x.CreateTime)
                   .HasColumnName("create_time");

            // 忽略字段
            builder.Property(x => x.TempField)
                   .IsIgnored();
        }
    }
}
```

**应用启动注册：**
```csharp
// Program.cs (ASP.NET Core 6+)
var builder = WebApplication.CreateBuilder(args);

// 注册实体配置
var configStore = new EntityConfigurationStore();
configStore.RegisterConfiguration(new UserEntityConfiguration());
// 注册更多配置...
EntityReflect.SetConfigurationStore(configStore);

var app = builder.Build();
// ...
```

---

## 7. 高级用法

### 7.1 复合主键

```csharp
// Attribute 方式不支持复合主键
// Fluent API 方式：
builder.HasKey(x => new { x.TenantId, x.UserId });
```

### 7.2 条件配置

```csharp
public class UserEntityConfiguration : EntityTypeConfiguration<UserEntity>
{
    private readonly bool _isMultiTenant;

    public UserEntityConfiguration(bool isMultiTenant = false)
    {
        _isMultiTenant = isMultiTenant;
    }

    public override void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("user_info");
        builder.HasKey(x => x.Id);

        // 根据条件动态配置
        if (_isMultiTenant)
        {
            builder.Property(x => x.TenantId).HasColumnName("tenant_id");
        }
    }
}
```

### 7.3 继承配置

```csharp
// 基础配置
public abstract class BaseEntityConfiguration<T> : EntityTypeConfiguration<T> 
    where T : BaseEntity
{
    public override void Configure(EntityTypeBuilder<T> builder)
    {
        // 所有实体的通用配置
        // 例如：软删除、创建时间等
    }
}

// 派生配置
public class UserEntityConfiguration : BaseEntityConfiguration<UserEntity>
{
    public override void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        base.Configure(builder); // 先应用基础配置
        
        // UserEntity 特有的配置
        builder.ToTable("user_info");
        builder.HasKey(x => x.Id);
    }
}
```

### 7.4 自动注册配置

```csharp
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEntityConfigurations(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        var configStore = new EntityConfigurationStore();
        
        foreach (var assembly in assemblies)
        {
            var configTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && 
                       ImplementsEntityTypeConfiguration(t));
            
            foreach (var configType in configTypes)
            {
                var configInstance = Activator.CreateInstance(configType);
                var entityType = GetEntityType(configType);
                
                var method = typeof(EntityConfigurationStore)
                    .GetMethod(nameof(EntityConfigurationStore.RegisterConfiguration))
                    .MakeGenericMethod(entityType);
                
                method.Invoke(configStore, new[] { configInstance });
            }
        }
        
        EntityReflect.SetConfigurationStore(configStore);
        return services;
    }
    
    private static bool ImplementsEntityTypeConfiguration(Type type)
    {
        return type.BaseType?.IsGenericType == true &&
               type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>);
    }
    
    private static Type GetEntityType(Type configType)
    {
        return configType.BaseType.GetGenericArguments()[0];
    }
}

// 使用方式
services.AddEntityConfigurations(typeof(Program).Assembly);
```

---

## 8. 注意事项

### 8.1 迁移顺序

```
┌─────────────────────────────────────────────────────────────┐
│                      推荐迁移顺序                            │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  1. 创建配置类（不删除 Attribute）                           │
│           ↓                                                 │
│  2. 注册配置，测试是否生效                                   │
│           ↓                                                 │
│  3. 运行单元测试，验证功能正常                               │
│           ↓                                                 │
│  4. 逐个移除实体类上的 Attribute                             │
│           ↓                                                 │
│  5. 完整回归测试                                            │
│           ↓                                                 │
│  6. 提交代码，更新文档                                       │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

### 8.2 优先级规则

- Fluent API 配置优先级 **高于** Attribute 配置
- 如果同时存在，Fluent API 配置会覆盖 Attribute 配置
- 建议迁移完成后统一移除 Attribute，避免混淆

### 8.3 性能影响

- Fluent API 配置在首次访问时解析，之后缓存
- 与 Attribute 方式性能相当
- 建议在应用启动时一次性注册所有配置

### 8.4 兼容性

- Fluent API 和 Attribute 可以混用（过渡期）
- 不同实体可以使用不同配置方式
- 最终目标是统一使用 Fluent API

---

## 9. 常见问题

### Q1: 迁移后 SQL 生成不正确？

**A:** 检查以下几点：
1. 配置类是否正确注册
2. `Configure` 方法是否被调用
3. 列名映射是否与数据库一致

```csharp
// 调试方法：检查元数据
var meta = EntityReflect.GetDefineInfoFromType(typeof(UserEntity));
Console.WriteLine($"Table: {meta.TableName}");
foreach (var col in meta.Columns)
{
    Console.WriteLine($"{col.PropertyName} -> {col.ColumnName}");
}
```

### Q2: 如何验证配置是否生效？

**A:** 使用以下代码验证：

```csharp
// 获取实体元数据
var meta = EntityReflect.GetDefineInfoFromType(typeof(UserEntity));

// 检查表名
Debug.Assert(meta.TableName == "user_info");

// 检查主键
var pkColumns = meta.Columns.Where(c => c.PrimaryKey).ToList();
Debug.Assert(pkColumns.Count == 1);
Debug.Assert(pkColumns[0].PropertyName == "Id");

// 检查自增列
var identityColumns = meta.Columns.Where(c => c.Identity).ToList();
Debug.Assert(identityColumns.Count == 1);
```

### Q3: 配置类没有被调用？

**A:** 确保在应用启动时正确注册：

```csharp
// ❌ 错误：忘记注册
var configStore = new EntityConfigurationStore();
// 缺少 RegisterConfiguration 调用
EntityReflect.SetConfigurationStore(configStore);

// ✅ 正确：注册所有配置
var configStore = new EntityConfigurationStore();
configStore.RegisterConfiguration(new UserEntityConfiguration());
configStore.RegisterConfiguration(new OrderEntityConfiguration());
EntityReflect.SetConfigurationStore(configStore);
```

### Q4: 如何处理大量实体？

**A:** 使用批量注册：

```csharp
// 扫描程序集，自动注册所有配置
var configStore = new EntityConfigurationStore();
var assembly = typeof(Program).Assembly;

var configTypes = assembly.GetTypes()
    .Where(t => t.IsClass && !t.IsAbstract && 
           t.BaseType?.IsGenericType == true &&
           t.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));

foreach (var configType in configTypes)
{
    var configInstance = Activator.CreateInstance(configType);
    var entityType = configType.BaseType.GetGenericArguments()[0];
    
    var method = typeof(EntityConfigurationStore)
        .GetMethod(nameof(EntityConfigurationStore.RegisterConfiguration))
        .MakeGenericMethod(entityType);
    
    method.Invoke(configStore, new[] { configInstance });
}

EntityReflect.SetConfigurationStore(configStore);
```

### Q5: 迁移过程中可以混用吗？

**A:** 可以。Vulcan ORM 支持过渡期混用：

```csharp
// 实体 A 使用 Fluent API
configStore.RegisterConfiguration(new UserEntityConfiguration());

// 实体 B 仍使用 Attribute（无需注册）
// [TableName("order_info")]
// public class OrderEntity : BaseEntity { ... }
```

但建议最终统一使用 Fluent API。

### Q6: 如何回滚？

**A:** 参见 [10. 回滚方案](#10-回滚方案)

---

## 10. 回滚方案

### 10.1 快速回滚

如果迁移出现问题，可以快速回滚：

```csharp
// 方法1：移除配置存储（回退到 Attribute）
EntityReflect.SetConfigurationStore(null);

// 方法2：注释掉注册代码
// configStore.RegisterConfiguration(new UserEntityConfiguration());
// EntityReflect.SetConfigurationStore(configStore);
```

### 10.2 代码回滚

```bash
# 使用 Git 回滚
git checkout backup/attribute-mapping

# 或者恢复备份文件
cp backup/entities/*.cs Entities/
```

### 10.3 渐进式回滚

如果只有部分实体有问题：

```csharp
// 只注册有问题的实体的配置
var configStore = new EntityConfigurationStore();

// 跳过有问题的配置
// configStore.RegisterConfiguration(new ProblemEntityConfiguration());

// 其他正常配置继续注册
configStore.RegisterConfiguration(new UserEntityConfiguration());
configStore.RegisterConfiguration(new OrderEntityConfiguration());

EntityReflect.SetConfigurationStore(configStore);

// 有问题的实体回退使用 Attribute
```

---

## 📚 附录

### A. 相关资源

- [Vulcan ORM 官方文档](https://github.com/xuanye/vulcan)
- [Dapper 官方文档](https://github.com/DapperLib/Dapper)
- [Entity Framework Fluent API 参考](https://docs.microsoft.com/ef/ef6/modeling/code-first/fluent/types-and-properties)

### B. 迁移检查清单

- [ ] 备份现有代码
- [ ] 创建配置类目录结构
- [ ] 为每个实体创建配置类
- [ ] 配置表名
- [ ] 配置主键
- [ ] 配置列映射
- [ ] 配置自增列
- [ ] 配置可空列
- [ ] 配置忽略字段
- [ ] 在启动时注册配置
- [ ] 运行单元测试
- [ ] 验证 SQL 生成正确
- [ ] 移除 Attribute
- [ ] 完整回归测试
- [ ] 更新文档

### C. 版本历史

| 版本 | 日期 | 变更 |
|------|------|------|
| 1.0 | 2026-03-21 | 初始版本 |

---

> **注意**: 本文档适用于 Vulcan.DapperExtensions 2.x 及以上版本。如有问题，请提交 Issue 或联系技术支持。
