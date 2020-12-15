# .Net Core 代码库

## 开发配置

### 端口

| 端口 | 解决方案 | 模块 | 位置 | 协议 |
| --- | --- | --- | --- | --- |
| 6111 | 6:YTS自定义 | 1:后台管理 | 1:前端H5页面 | 1:Https协议 |
| 6112 | 6:YTS自定义 | 1:后台管理 | 1:前端H5页面 | 2:Http协议 |
| 6121 | 6:YTS自定义 | 1:后台管理 | 2:API | 1:Https协议 |
| 6122 | 6:YTS自定义 | 1:后台管理 | 2:API | 2:Http协议 |

## 发布

### 发布命令模板

#### Window

```shell
dotnet publish -o <盘符>:\wwwroot\YTSCSharpDotNetCore\<项目名称>
```

#### Linux

```shell
dotnet publish -o /var/wwwroot/YTSCSharpDotNetCore/<项目名称>
```

### 本地常用命令

```shell
dotnet publish -o D:\wwwroot\YTSCSharpDotNetCore\YTS.AdminWeb
dotnet publish -o D:\wwwroot\YTSCSharpDotNetCore\YTS.AdminWebApi
```

## 数据库更新命令

```shell
cd ../YTS.AdminWebApi
dotnet ef dbcontext list
dotnet ef database drop --force
cd ../YTS.Shop
dotnet ef migrations add InitialCreate --startup-project ../YTS.AdminWebApi
dotnet ef database update --startup-project ../YTS.AdminWebApi
dotnet ef migrations remove --startup-project ../YTS.AdminWebApi
```

## 发布包到NuGet

```shell
dotnet pack // 打包

dotnet nuget push <PackagesPath> --api-key <ApiKey> --source https://api.nuget.org/v3/index.json
```

## 学习链接

* [微软官网 - .NET Core 指南](https://docs.microsoft.com/zh-cn/dotnet/core/)
* [微软官网 - .NET Core SDK 概述](https://docs.microsoft.com/zh-cn/dotnet/core/sdk)
* [微软官网 - Entity Framework Core](https://docs.microsoft.com/zh-cn/ef/core/)
* [.NET Core简单读取json配置文件](https://www.jb51.net/article/137517.htm)
* [.net core 使用Newtonsoft.Json 读取Json文件数据](https://blog.csdn.net/liwan09/article/details/102952990)
* [.net core读取json格式的配置文件](https://www.cnblogs.com/dotnet261010/p/10172961.html)
* [Sqlite Browser](https://sqlitebrowser.org/)
* [什么是 JWT -- JSON WEB TOKEN](https://www.jianshu.com/p/576dbf44b2ae)
* [ASP.Net Core 3.1 中使用JWT认证](https://www.cnblogs.com/liuww/p/12177272.html)
* [nuget.org 无法加载源 https://api.nuget.org/v3/index.json 的服务索引](https://www.cnblogs.com/shapaozi/archive/2017/10/31/7764469.html)
* [jquery ajax设置header的两种方式](https://blog.csdn.net/shjavadown/article/details/51213342)
* [AJAX请求中出现OPTIONS请求](https://www.cnblogs.com/wanghuijie/p/preflighted_request.html)
* [OPTIONS 方法在跨域请求（CORS）中的应用](https://blog.csdn.net/qizhiqq/article/details/71171916)
* [dotNET Core Web API+JWT(Bearer Token)认证+Swagger UI](https://blog.csdn.net/qq_35904166/article/details/84591227)
* [EF Core 入门](https://docs.microsoft.com/zh-cn/ef/core/get-started/?tabs=netcore-cli)
* [ASP.NET Core 中的 Razor 页面和 Entity Framework Core - 第 1 个教程（共 8 个）](https://docs.microsoft.com/zh-cn/aspnet/core/data/ef-rp/intro?view=aspnetcore-3.1&tabs=visual-studio-code)
* [SQLite Browser 数据库浏览器](https://sqlitebrowser.org/)
* [6个宝藏级Vue管理后台框架 必须收藏](https://zhuanlan.zhihu.com/p/91825869)
* [asp.net core 集成JWT（一）](https://www.cnblogs.com/7tiny/p/11012035.html)
* [从壹开始前后端分离【 .NET Core2.2/3.0 +Vue2.0 】框架之五 || Swagger的使用 3.3 JWT权限验证【必看】 - 老张的哲学 - 博客园](https://www.cnblogs.com/laozhang-is-phi/p/9511869.html#autoid-4-0-0)
* [easyUI datagrid携带token](https://blog.csdn.net/zcwforali/article/details/79866181)
* [easyui jquery ajax的全局设置token](https://blog.csdn.net/mutourenoo/article/details/84921154)
* [ajax 全局设置token，并且判断是或否登录 $.ajaxSetup设置](https://blog.csdn.net/qq_32674347/article/details/88415757)
* [ASP.NET CORE系列【六】Entity Framework Core 之数据迁移](https://www.cnblogs.com/shumin/p/8877297.html)
* [Jquery EasyUI插件](http://www.jeasyui.net/plugins)
* [asp.net core合并压缩资源文件引发的学习之旅](https://www.cnblogs.com/morang/p/7604612.html)
* [ASP.NET Core 中的捆绑和缩小静态资产](https://docs.microsoft.com/zh-cn/aspnet/core/client-side/bundling-and-minification?view=aspnetcore-3.1&tabs=netcore-cli)
* [设置 ASP.NET Core Web API 中响应数据的格式](https://docs.microsoft.com/zh-cn/aspnet/core/web-api/advanced/formatting?view=aspnetcore-3.1)
* [.net core3.1 web api中使用newtonsoft替换掉默认的json序列化组件](https://www.cnblogs.com/shapman/p/12232640.html)
* [NuGet.org 无法访问的解决方法](https://blog.csdn.net/weixin_34242819/article/details/85688216)
* [windows服务器：编写bat脚本,创建定时任务](https://blog.csdn.net/eyeofeagle/article/details/88992435)
* [windows建立定时任务执行bat脚本](https://blog.csdn.net/slibra_L/article/details/89227736)
* [命令创建定时任务及bat脚本](https://blog.csdn.net/qq_31176861/article/details/90901336)
* [windows 10添加定时任务](https://www.cnblogs.com/wensiyang0916/p/5773828.html)
* [依赖注入[1]: 控制反转](https://www.cnblogs.com/artech/p/net-core-di-01.html)
* [依赖注入[2]: 基于IoC的设计模式](https://www.cnblogs.com/artech/p/net-core-di-02.html)
* [依赖注入[3]: 依赖注入模式](https://www.cnblogs.com/artech/p/net-core-di-03.html)
* [依赖注入[4]: 创建一个简易版的DI框架[上篇]](https://www.cnblogs.com/artech/p/net-core-di-04.html)
* [依赖注入[5]: 创建一个简易版的DI框架[下篇]](https://www.cnblogs.com/artech/p/net-core-di-05.html)
* [依赖注入[6]: .NET Core DI框架[编程体验]](https://www.cnblogs.com/artech/p/net-core-di-06.html)
* [依赖注入[7]: .NET Core DI框架[服务注册]](https://www.cnblogs.com/artech/p/net-core-di-07.html)
* [依赖注入[8]: .NET Core DI框架[服务消费]](https://www.cnblogs.com/artech/p/net-core-di-08.html)
* [深入理解net core中的依赖注入、Singleton、Scoped、Transient（一）](https://www.cnblogs.com/gdsblog/p/8465101.html)
* [深入理解net core中的依赖注入、Singleton、Scoped、Transient（二）](https://www.cnblogs.com/gdsblog/p/8465109.html)
* [深入理解net core中的依赖注入、Singleton、Scoped、Transient（三）](https://www.cnblogs.com/gdsblog/p/8465113.html)
* [深入理解net core中的依赖注入、Singleton、Scoped、Transient（四）](https://www.cnblogs.com/gdsblog/p/8465401.html)
* [.NET Core ASP.NET Core Basic 1-2 控制反转与依赖注入](https://www.cnblogs.com/WarrenRyan/p/11444398.html)
* [.NET Core容器化@Docker](https://www.jianshu.com/p/23465dc86d3e)
* [asp.net core webapi 使用ef 对mysql进行增删改查，并生成Docker镜像构建容器运行](https://blog.csdn.net/weixin_30908649/article/details/97854371)
* [Docker使用docker-compose.yml构建Asp.Net Core和Mysql镜像并与Mysql数据库通信](https://my.oschina.net/u/4357854/blog/3566361)
* [ASP.NET Core 实战：使用 Docker 容器化部署 ASP.NET Core + MySQL + Nginx](https://www.cnblogs.com/danvic712/p/10566750.html)
* [配置 DbContext](https://docs.microsoft.com/zh-cn/ef/core/miscellaneous/configuring-dbcontext)
* [.NET Core 跨平台执行命令、脚本](https://www.cnblogs.com/stulzq/p/9074965.html)
* [dotnet core（C#）下读取ANSI（GB2312）编码的文本](https://blog.csdn.net/sunnyzls/article/details/104751426)
* [.NET Core 轻量级模板引擎 Mustachio](https://cloud.tencent.com/developer/article/1547299)

### FreeSql 学习

* [FreeSql GitHub 源代码 Wiki](https://github.com/dotnetcore/FreeSql/wiki)
* [FreeSql（一）入门](https://www.cnblogs.com/FreeSql/p/11531300.html)
* [FreeSql（二）自动迁移实体](https://www.cnblogs.com/FreeSql/p/11531301.html)
* [FreeSql（三）实体特性](https://www.cnblogs.com/FreeSql/p/11531302.html)
* [FreeSql（四）实体特性 Fluent Api](https://www.cnblogs.com/FreeSql/p/11531304.html)
* [FreeSql（五）插入数据](https://www.cnblogs.com/FreeSql/p/11531306.html)
* [FreeSql（六）批量插入数据](https://www.cnblogs.com/FreeSql/p/11531309.html)
* [FreeSql（七）插入数据时忽略列](https://www.cnblogs.com/FreeSql/p/11531316.html)
* [FreeSql（八）插入数据时指定列](https://www.cnblogs.com/FreeSql/p/11531318.html)
* [FreeSql（九）删除数据](https://www.cnblogs.com/FreeSql/p/11531320.html)
* [FreeSql（十）更新数据](https://www.cnblogs.com/FreeSql/p/11531321.html)
* [FreeSql（十一）更新数据 Where](https://www.cnblogs.com/FreeSql/p/11531324.html)
* [FreeSql（十二）更新数据时指定列](https://www.cnblogs.com/FreeSql/p/11531327.html)
* [FreeSql（十三）更新数据时忽略列](https://www.cnblogs.com/FreeSql/p/11531334.html)
* [FreeSql（十四）批量更新数据](https://www.cnblogs.com/FreeSql/p/11531335.html)
* [FreeSql（十五）查询数据](https://www.cnblogs.com/FreeSql/p/11531339.html)
* [FreeSql（十六）分页查询](https://www.cnblogs.com/FreeSql/p/11531341.html)
* [FreeSql（十七）联表查询](https://www.cnblogs.com/FreeSql/p/11531346.html)
* [FreeSql（十八）导航属性](https://www.cnblogs.com/FreeSql/p/11531352.html)
* [FreeSql（十九）多表查询](https://www.cnblogs.com/FreeSql/p/11531362.html)
* [FreeSql（二十）多表查询 WhereCascade](https://www.cnblogs.com/FreeSql/p/11531372.html)
* [FreeSql（二十一）查询返回数据](https://www.cnblogs.com/FreeSql/p/11531376.html)
* [FreeSql（二十二）Dto 映射查询](https://www.cnblogs.com/FreeSql/p/11531381.html)
* [FreeSql（二十三）分组、聚合](https://www.cnblogs.com/FreeSql/p/11531384.html)
* [FreeSql（二十四）Linq To Sql 语法使用介绍](https://www.cnblogs.com/FreeSql/p/11531392.html)
* [FreeSql（二十五）延时加载](https://www.cnblogs.com/FreeSql/p/11531395.html)
* [FreeSql（二十六）贪婪加载 Include、IncludeMany](https://www.cnblogs.com/FreeSql/p/11531404.html)
* [FreeSql（二十七）将已写好的 SQL 语句，与实体类映射进行二次查询](https://www.cnblogs.com/FreeSql/p/11531416.html)
* [FreeSql（二十八）事务](https://www.cnblogs.com/FreeSql/p/11531423.html)
* [FreeSql（二十九）Lambda 表达式](https://www.cnblogs.com/FreeSql/p/11531425.html)
* [FreeSql（三十）读写分离](https://www.cnblogs.com/FreeSql/p/11531430.html)
* [FreeSql（三十一）分表分库](https://www.cnblogs.com/FreeSql/p/11531435.html)
* [FreeSql（三十二）Aop](https://www.cnblogs.com/FreeSql/p/11531471.html)
* [FreeSql（三十三）CodeFirst 类型映射](https://www.cnblogs.com/FreeSql/p/11531543.html)
* [FreeSql（三十四）CodeFirst 迁移说明](https://www.cnblogs.com/FreeSql/p/11531550.html)
* [FreeSql（三十五）CodeFirst 自定义特性](https://www.cnblogs.com/FreeSql/p/11531576.html)
