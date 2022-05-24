using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using YTS.Shop;

namespace YTS.WebApi
{
    /// <summary>
    /// 数据库相关服务扩展
    /// </summary>
    public static class DataBaseServiceExtensions
    {
        /// <summary>
        /// 注入主程序数据库处理
        /// </summary>
        public static void EnterHostDataBases(this IHost host)
        {
            host.CreateDbIfNotExists<YTSEntityContext, YTS.AdminWebApi.Program>();
        }

        /// <summary>
        /// 创建数据库, 如果数据库不存在的话
        /// </summary>
        private static void CreateDbIfNotExists<Db, P>(this IHost host) where Db : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<Db>();
                    context.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<P>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }

        /// <summary>
        /// 注入服务 数据库设置
        /// </summary>
        public static void EnterServiceDataBases(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<YTSEntityContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString(ApiConfig.DataBase_YTSEntity)));

        }
    }
}
