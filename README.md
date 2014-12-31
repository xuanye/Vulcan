# vulcan

---

Vulcan 基于[Dapper.NET][1]的数据库链接托管类库，支持数据库链接管理透明化，支持事务包装，内置支持Mysql和MSSQL数据库，可扩展支持其他数据库，只要Dapper支持即可

## 特性

1. ORM功能基于Dapper.NET和代码生成工具，性能高效
2. 数据库链接托管，使用者不需要关心合适开启链接和关闭连接
3. 事务托管，支持嵌套（MySql数据库不支持）
4. 默认支持Mysql和Sql Server，可快速扩展支持其他关系型数据库（只要Dapper支持即可）
5. 支持Web程序和桌面应用程序
6. 轻量，开源


## 使用

```
PM> Install-Package Vulcan
```


待完善文档....


  [1]: https://github.com/StackExchange/dapper-dot-net