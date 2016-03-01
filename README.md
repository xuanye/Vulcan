# vulcan

* * *

Vulcan 基于[Dapper.NET][1]的数据库链接托管类库，支持数据库链接管理透明化，支持事务包装，内置支持Mysql和MSSQL数据库，可扩展支持其他数据库，只要Dapper支持即可

## 1. 特性

1.  ORM功能基于Dapper.NET和代码生成工具，性能高效
2.  数据库链接托管，使用者不需要关心合适开启链接和关闭连接
3.  事务托管，支持嵌套（MySql数据库不支持）
4.  默认支持 Mysql 和 SQL Server，可快速扩展支持其他关系型数据库（只要Dapper支持即可）
5.  支持Web程序和桌面应用程序
6.  轻量，开源

## 2. 安装

> PM > Install-Package Vulcan

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

`Vulcan` 中内部已经实现了SQl SERVER 的ConnectionFactory，如果你使用Mysql的话 需要，引用`MySql.Data`这个DLL，并将下面的类复制到你的应用程序中：
```CSharp
   public class MySqlConnectionFactory : ConnectionFactory
    {
        protected override IDbConnection CreateDefaultDbConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }
    }
```
然后设置：
```CSharp
ConnectionFactory.Default = new MySqlConnectionFactory();
```
至于为什么不把 `MySqlConnectionFactory` 直接放到Vulcan内部?其实只要是为了减少外部引用，毕竟使用.NET的程序大多数是使用SQL SERVER的。

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
public class BaseRepository : Vulcan.DataAccess.ORMapping.MSSql.MSSqlRepository
{
    public BaseRepository()
        : this(ConStrHelper.maindb) // 默认使用的数据库
    {

    }
    public BaseRepository(string dbKey)
        : base(ConStrHelper.GetConnectionStringByKey(dbKey))  // 配置单独的链接字符串KEY
    {

    }
}

public class ConStrHelper
{
    internal const string maindb = "master";
    static Dictionary<string,string> _cache = new Dictionary<string,string>();
    internal static string GetConnectionStringByKey(string dbkey)
    {
        if(_cache.ContainsKey(dbkey)){
            return _cache[dbkey];
        }
        string encontr = System.Configuration.ConfigurationManager.ConnectionStrings[dbkey].ConnectionString;
        string constr = Vulcan.Common.CryptographyManager.AESDecrypt(encontr, "私钥"); // 可使用自己的加解密方式，或者不加密
        _cache.Add(dbkey, constr);
        return constr;
    }
}
```


### 3.2 数据库实体生成
就我个人的情况来看，一般系统开发都是先设计数据库，然后再开始编码， 而编写数据库实体又是重复又枯燥的部分，所以可以使用代码生成器来批量生成数据库实体。世面上有很多类似的工具，这里提供了可以使用于`Vulcan`的代码生成模板（T4模板）。
这里提供[MySql][3]和[SQL SERVER][4] 两个版本
将对应数据库服务的三个文件 `Base.ttinclude`,`EntityGenerate.tt`,`MSSQL.ttinclude` 复制到自己的实体项目中，打开`EntityGenerate.tt`文件 ，需要修改两处，代码如下：

```
﻿<#@ template debug="true" hostSpecific="true" #>
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

```
public class UserInfoService
{
  private UserInfoRepository _repo ;
  public UserInfoService()
  {
     _repo = new UserInfoRepository();
  }
  public List<UserInfo> QueryUserByAge(int age)
  {
    return _repo.QueryUserByAge(age);  //不用考虑数据库链接的打开和关闭
  }
  //.. 其他业务方法；
}


public class UserInfoRepository : BaseRepository
{
    public UserInfoRepository()
        :base() 
    {

    }
    public List<UserInfo> QueryUserByAge(int age)
    {
        string sql = "SELECT * FROM USER_INFO WHERE Age>@Age";
        base.Query<Userinfo>(sql,new {Age = age});
    }
}

```

### 3.4 事务
同 `Repository` 一样，建议在自己的项目中新建 一个`TransScope`类，代码如下，用于处理链接字符串的逻辑
```
public class TransScope : Vulcan.DataAccess.TransScope
{
    public TransScope()
        : this(ConStrHelper.maindb)
    {

    }
    public TransScope(string dbKey)
        : base(ConStrHelper.GetConnectionStringByKey(dbKey))
    {

    }
}
```

实际使用实务：
```
public class UserInfoService
{
  private UserInfoRepository _repo ;
  private LogRepository _logrepo ;
  public UserInfoService()
  {
     _repo = new UserInfoRepository();
     _logrepo = new LogRepository();
  }
  public List<UserInfo> SaveUser(UserInfo uitem)
  {
        using(TransScope scope = new TransScope())
        {
           
            // 一系列的数据库操作..
            if（uitem.Id>0){
                _repo.Insert(uitem);
            }
            else{
               _repo.Update(uitem);
            }
            Log log =new Log();
            //... 赋值
            
            // 保存
            _logrepo.SaveLog(log);
            
            // ... 其他数据库操作
            
            // 提交事务
            scope.Commit();
        }
  }
  //.. 其他业务方法；
}
```




  [1]: https://github.com/StackExchange/dapper-dot-net
  [2]: https://github.com/StackExchange/dapper-dot-net
  [3]: https://github.com/xuanye/vulcan/tree/master/sample/template/mysql
  [4]: https://github.com/xuanye/vulcan/tree/master/sample/template/sqlserver