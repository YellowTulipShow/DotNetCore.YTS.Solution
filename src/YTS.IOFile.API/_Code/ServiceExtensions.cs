using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

using Swashbuckle.AspNetCore;

namespace YTS.IOFile.API
{
    /// <summary>
    /// 各服务调用扩展静态帮助类
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 注入服务 Controllers 配置
        /// </summary>
        public static void EnterServiceControllers(this IServiceCollection services)
        {
            // 增加 Controller 注册启用
            services.AddControllers(option =>
            {
                // 关闭 启用端点路由
                option.EnableEndpointRouting = false;
            });
        }

        /// <summary>
        /// 配置 JSON 序列化设置
        /// </summary>
        public static void EnterServiceJson(this IServiceCollection services)
        {
            services.AddMvc().AddJsonOptions(option =>
            {
                // 获取或设置一个值，该值确定反序列化期间属性名称是否使用不区分大小写的比较。 默认值为false。
                option.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
                // 获取或设置一个值，该值指定用于将对象上的属性名称转换为另一种格式（例如骆驼套）的策略，或者为null以保持属性名称不变。
                option.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
        }

        /// <summary>
        /// 应用程序启用 API 路由路径模板
        /// </summary>
        public static void StartEnableRoute(this IApplicationBuilder app)
        {
            // 启用路由
            app.UseRouting();

            // 使用MVC
            app.UseMvc(routes =>
            {
                //// 配置默认MVC路由模板
                //routes.MapRoute(
                //    name: "default",
                //    template: ApiConfig.APIRoute);
                //// 配置默认MVC路由模板
                //routes.MapRoute(
                //    name: "ioapi",
                //    template: "/ioapi" + ApiConfig.APIRoute);
            });
        }

        /// <summary>
        /// 注入服务 跨域配置
        /// </summary>
        public static void EnterServiceCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(ApiConfig.CorsName, builder =>
                {
                    builder.WithOrigins("*").WithHeaders("*");
                });
            });
        }
        /// <summary>
        /// 应用程序启用跨域策略
        /// </summary>
        public static void StartEnableCors(this IApplicationBuilder app)
        {
            // 使用跨域策略
            app.UseCors(ApiConfig.CorsName);
        }

    }
}
