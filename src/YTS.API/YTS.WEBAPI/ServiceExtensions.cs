using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace YTS.WEBAPI
{
    /// <summary>
    /// 各服务调用扩展静态帮助类
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 注入服务 Controllers 配置
        /// </summary>
        public static void EnterServiceControllers(this IServiceCollection services, Action<MvcOptions> configMVC = null, Action<MvcNewtonsoftJsonOptions> configJSON = null)
        {
            // 增加 Controller 注册启用
            var mvc = services.AddControllers(option =>
            {
                // 关闭 启用端点路由
                option.EnableEndpointRouting = false;
                configMVC?.Invoke(option);
            });
            mvc.AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";                        // 设置时间格式
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;            // 忽略循环引用
                //options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); // 数据格式首字母小写
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();                // 数据格式按原样输出
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;                    // 忽略空值
                configJSON?.Invoke(options);
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
            app.UseMvc();
        }
    }
}
