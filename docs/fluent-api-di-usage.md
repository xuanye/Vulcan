# 在依赖注入中使用 Fluent API 配置

## 1. 定义实体配置

```csharp
using Vulcan.DapperExtensions.ORMapping;
using Vulcan.DapperExtensions.ORMapping.Configuration;

// 实体类
public class User : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
}

// 配置类
public class UserConfiguration : EntityTypeConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("sys_users");
        
        builder.Property(x => x.Id)
            .HasColumnName("user_id")
            .IsPrimaryKey()
            .IsIdentity();
            
        builder.Property(x => x.Name)
            .HasColumnName("user_name");
            
        builder.Property(x => x.Email)
            .HasColumnName("email");
            
        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at");
    }
}
```

## 2. 在 Startup.cs 中注册

```csharp
using Microsoft.Extensions.DependencyInjection;
using Vulcan.DapperExtensions.Extensions;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // 方式 1: 自动扫描当前程序集中的所有配置
        services.AddVulcan(typeof(Startup).Assembly);
        
        // 方式 2: 扫描多个程序集
        services.AddVulcan(
            typeof(Startup).Assembly,
            typeof(UserConfiguration).Assembly
        );
        
        // 方式 3: 手动配置
        services.AddVulcan(options =>
        {
            options.AutoScanAssemblies.Add(typeof(Startup).Assembly);
        });
    }
}
```

## 3. 使用 Repository

```csharp
using Vulcan.DapperExtensions;
using Vulcan.DapperExtensions.Contract;
using Vulcan.DapperExtensions.ORMapping;

public class UserRepository : BaseRepository
{
    public UserRepository(
        IConnectionManagerFactory connectionManagerFactory,
        string connectionString) 
        : base(connectionManagerFactory, connectionString)
    {
    }
    
    public async Task<User> GetByIdAsync(int id)
    {
        const string sql = "SELECT * FROM sys_users WHERE user_id = @Id";
        return await GetAsync<User>(sql, new { Id = id });
    }
    
    public async Task<int> InsertAsync(User user)
    {
        return await InsertAsync<int>(user);
    }
}

// 在 Controller 或 Service 中使用
public class UserService
{
    private readonly UserRepository _repository;
    
    public UserService(
        IConnectionManagerFactory connectionManagerFactory,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        _repository = new UserRepository(connectionManagerFactory, connectionString);
    }
    
    public async Task<User> GetUserByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
}
```

## 4. 完整的 DI 注册示例

```csharp
// Program.cs (ASP.NET Core 6+)
var builder = WebApplication.CreateBuilder(args);

// 注册 Vulcan ORM
builder.Services.AddVulcan(typeof(Program).Assembly);

// 注册 ConnectionManagerFactory
builder.Services.AddSingleton<IRuntimeContextStorage, AsyncLocalStorage>();
builder.Services.AddSingleton<IConnectionManagerFactory, ConnectionManagerFactory>();

// 注册 Repository
builder.Services.AddScoped<UserRepository>(provider =>
{
    var factory = provider.GetRequiredService<IConnectionManagerFactory>();
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new UserRepository(factory, connectionString);
});

var app = builder.Build();
app.Run();
```

## 5. 配置优先级

Fluent API 配置优先于 Attribute 配置：

```csharp
// 使用 Attribute 标记（旧方式，仍然支持）
[TableName("old_users")]
public class User : BaseEntity
{
    [PrimaryKey]
    [Identity]
    [MapField("old_id")]
    public int Id { get; set; }
}

// 使用 Fluent API 配置（新方式，优先级更高）
public class UserConfiguration : EntityTypeConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("new_users");  // 这个会覆盖 Attribute 的配置
        builder.Property(x => x.Id).HasColumnName("new_id");
    }
}
```

## 6. 手动注册配置（不使用自动扫描）

```csharp
// 如果不想使用自动扫描，可以手动注册
var store = new EntityConfigurationStore();
store.RegisterConfiguration(new UserConfiguration());
store.RegisterConfiguration(new OrderConfiguration());

// 设置全局配置存储
EntityReflect.SetConfigurationStore(store);

// 注册到 DI
services.AddSingleton<IEntityConfigurationStore>(store);
```
