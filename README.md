# Vulcan
---

Vulcan 基于[Dapper.NET][1]的数据库链接托管类库，支持数据库链接管理透明化，支持事务包装，内置支持Mysql和MSSQL数据库，可扩展支持其他数据库，只要Dapper支持即可

## 1. 特性

1.  ORM功能基于Dapper.NET和代码生成工具，性能高效
2.  数据库链接托管，使用者不需要关心合适开启链接和关闭连接
3.  事务托管，支持嵌套（MySql数据库不支持）
4.  默认支持 Mysql 和 SQL Server，可快速扩展支持其他关系型数据库（只要Dapper支持即可）
5.  支持Web程序和桌面应用程序
6.  轻量，开源

## 2. 安装

$> dotnet add package Vulcan.DataAcces

## 3. 使用
`Vulcan` 会提供一个数据库操作的基类，封装了一些基本的增删改查的方法。
包括：

```CSharp
// 插入
public long Insert(Vulcan.DataAccess.ORMapping.BaseEntity entity)
// 更新
public int Update(Vulcan.DataAccess.ORMapping.BaseEntity entity)
// 批量插入
public int BatchInsert<T>(List<T> list)
	where T : Vulcan.DataAccess.ORMapping.BaseEntity
// 批量更新
public int BatchUpdate<T>(List<T> list)
	where T : Vulcan.DataAccess.ORMapping.BaseEntity

// 执行SQL
protected int Excute(string sql, object paras)
// 查询列表
protected List<T> Query<T>(string sql, object paras)
// 获取某一条记录
protected T Get<T>(string sql, object paras)

// 执行存储过程
protected int SPExcute(string spName, object paras)
// 存储过程返回一个对象
protected T SPGet<T>(string spName, object paras)
// 存储过程返回一个列表
protected List<T> SPQuery<T>(string spName, object paras)
```

`Vulcan.DataAccess.ORMapping.BaseEntity` 是用于自动生成更新和插入SQL语句的基类，更新和插入类的方法需要使用，也可以自行实现SQL语句。

更多的用法，可参考 [Dapper][2] 中 connection 扩展方法的使用.

### 3.1 选择自己的数据库服务
在应用程序启动时需要设置 `Vulcan` 的Connection工厂的类型：
```CSharp
ConnectionFactory.Default = new SqlConnectionFactory();
```

`Vulcan` 中内部已经实现了SQl SERVER 的ConnectionFactory，如果你使用Mysql的话 需要，添加引用`MySql.Data`，并将下面的类复制到你的应用程序中：

```CSharp
 public class MySqlConnectionFactory: IConnectionFactory
 {
    public IDbConnection CreateDbConnection(string connectionString)
    {
       return new MySqlConnection(connectionString);
    }
 }

```

然后在Startup设置：

```CSharp

 services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
 services.AddSingleton<IRuntimeContextStorage, AspNetCoreContext>();
 services.AddSingleton<IConnectionFactory, MySqlConnectionFactory>();
 services.AddSingleton<IConnectionManagerFactory, ConnectionManagerFactory>();

```



### 3.2 基类
`Vulcan` 内部已经实现了Mysql 和SQL Server两个版本，在开始使用前，需要选择合适的基类去继承。

**Mysql**

```CSharp
using Vulcan.DataAccess.ORMapping.MySql;
public class UserInfoRepository : MySqlRepository
{
    public UserInfoRepository(string constr)
        :base(constr)
    {

    }
    public List<Userinfo> QueryUser(int age)
    {
        string sql = "SELECT * FROM USER_INFO WHERE Age>@Age";
        base.Query<Userinfo>(sql,new {Age =age});
    }
}
```

**SQL SERVER**

```CSharp
using Vulcan.DataAccess.ORMapping.MSSql;

public class UserInfoRepository : MSSqlRepository
{
    public UserInfoRepository(string constr)
        :base(constr)
    {

    }
    // 实际的代码 ...
}
```


一般建议在实际的项目中，建立一个新的基类，用于继承上面两个类，并且在构造函数中处理链接字符串相关的逻辑（如在实际使用中，链接字符串是加密配置在配置文件中的，那就需要解密），示例如下：
```

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

//实际的业务数据访问类
    public class UserManageRepository : BaseRepository, IUserManageRepository
    {

        public UserManageRepository(IConnectionManagerFactory factory,
           IOptions<DBOption> Option,
           ILoggerFactory loggerFactory) : base(factory, Option.Value.Master, loggerFactory)
        {

        }
   }

```


### 3.2 数据库实体生成

就我个人的情况来看，一般系统开发都是先设计数据库，然后再开始编码， 而编写数据库实体又是重复又枯燥的部分，所以可以使用代码生成器来批量生成数据库实体。世面上有很多类似的工具，这里提供了可以使用于`Vulcan`的代码生成模板（T4模板）。
这里提供[MySql][3]和[SQL SERVER][4] 两个版本
将对应数据库服务的三个文件 `Base.ttinclude`,`EntityGenerate.tt`,`MSSQL.ttinclude` 复制到自己的实体项目中，打开`EntityGenerate.tt`文件 ，需要修改两处，代码如下：

```CSharp

?<#@ template debug="true" hostSpecific="true" #>
<#@ output extension=".cs" #>
<#@ include file="Base.ttinclude" #>
<#@ include file="MSSql.ttinclude"  #>

<#
 
    //修改成你自己的数据库地址，应该是开发库的地址
	ConnectionString = "Data Source=(local);Initial Catalog=testdb;Persist Security Info=True;User ID=sa;Password=123456";
    Namespace       = "VulcanSample.Entity"; //修改生成实体的命名空间
    DataContextName = "DataContext";
	BaseEntityClass = "BaseEntity";
	RenderForeignKeys = false;
	RenderBackReferences = false;
    GenerateModel();

#>

```

保存后即可生成对应的实体代码。

### 3.3 在Service中使用Repository

```CSharp
 public class UserManageService: IUserManageService
    {
        private readonly IUserManageRepository _repo;
        private readonly IOrgManageService _orgService;
        public UserManageService(IUserManageRepository repo, IOrgManageService orgService)
        {
            this._repo = repo;
            this._orgService = orgService;
        }

        public Task<PagedList<IUserInfo>> QueryUserList(string orgCode, string qText, PageView view)
        {
            orgCode = Utility.ClearSafeStringParma(orgCode);
            qText = Utility.ClearSafeStringParma(qText);

            return _repo.QueryUserList(orgCode, qText, view);
        }
}

```

### 3.4 事务


使用实务：


```CSharp
   public async Task<int> SaveRole(IRoleInfo entity, int type)
        {
            int ret = -1;
            using (var scope = this._repo.BeginTransScope()) //使用当前链接创建事务
            {
                if (type == 1) // 新增
                {
                    if (!entity.RoleCode.StartsWith(entity.AppCode))
                    {
                        entity.RoleCode = entity.AppCode + "_" + entity.RoleCode;
                    }
                    // 校验
                    bool result = await this._repo.CheckCode(entity.RoleCode);
                    if (!result)
                    {
                        ret = -1;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(entity.ParentCode)) // 根组织
                        {                          
                            entity.Left = 1;
                            entity.Right = entity.Left + 1;
                        }
                        else
                        {
                            IRoleInfo pRole = await _repo.GetRole(entity.ParentCode);
                            if (pRole == null)
                            {
                                throw new BizException("父角色不存在，请检查后重新保存");
                            }

                            await _repo.UpdateRolePoint(entity.AppCode, pRole.Right);
                            entity.Left = pRole.Right;
                            entity.Right = entity.Left + 1;
                        }

                        await this._repo.AddRole(entity);
                        ret = 1;
                    }
                }
                else
                {
                    ret = await this._repo.UpdateRole(entity);
                }
                scope.Complete(); //提交事务
            }
            return ret;
        }

```


# 3.5 反馈

更多问题可以到issues中反馈，或者查看完成的sample示例，查看使用


  [1]: https://github.com/StackExchange/dapper-dot-net
  [2]: https://github.com/StackExchange/dapper-dot-net
  [3]: https://github.com/xuanye/vulcan/tree/master/sample/template/mysql
  [4]: https://github.com/xuanye/vulcan/tree/master/sample/template/sqlserver