using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

using Swashbuckle.AspNetCore.Filters;

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
            })
            .AddNewtonsoftJson(option =>
            {
                option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                option.SerializerSettings.ContractResolver = new DefaultContractResolver();

                option.SerializerSettings.Converters.Add(new IsoDateTimeConverter()
                {
                    DateTimeFormat = "yyyy-MM-dd HH:mm:ss",
                });
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
                // 配置默认MVC路由模板
                routes.MapRoute(
                    name: "default",
                    template: ApiConfig.APIRoute);
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

        /// <summary>
        /// 注入服务 Swagger API 浏览配置
        /// </summary>
        public static void EnterServiceSwagger(this IServiceCollection services, IConfiguration conf)
        {
            var swaggerInfo = conf.GetSection(ApiConfig.APPSettingName_SwaggerInfo);
            var model = swaggerInfo.Get<SwaggerInfo>();
            // Register the Swagger generator, defining 1 or more Swagger documents
            // 注册Swagger生成器，定义1个或多个Swagger文档
            services.AddSwaggerGen(c =>
            {
                // 配置 v1文档
                c.SwaggerDoc(model.Version, new OpenApiInfo
                {
                    Version = model.Version,
                    Title = model.Title,
                    Description = model.Description,
                    Contact = new OpenApiContact
                    {
                        Name = model.Contact.Name,
                        Email = model.Contact.Email,
                        Url = new Uri(model.Contact.Url),
                    },
                    License = new OpenApiLicense
                    {
                        Name = model.License.Name,
                        Url = new Uri(model.License.Url),
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                // 设置Swagger JSON和UI的注释路径。读取代码XML注释文档
                var name = Assembly.GetExecutingAssembly().GetName().Name;
                var xmlFile = $"{name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }
        /// <summary>
        /// 应用程序启用 Swagger API 文档浏览
        /// </summary>
        public static void StartEnableSwagger(this IApplicationBuilder app, IConfiguration conf)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            // 启用中间件以将生成的Swagger用作JSON端点。
            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((open_api_doc, http_request) =>
                {
                });
            });

            string VirtualDirectory = conf.GetValue<string>("VirtualDirectory")
                ?.Trim()?.TrimEnd('/') ?? string.Empty;

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            // 启用中间件以提供swagger-ui（HTML，JS，CSS等），
            // 指定Swagger JSON端点。
            app.UseSwaggerUI(c =>
            {
                if (!string.IsNullOrEmpty(VirtualDirectory))
                {
                    c.SwaggerEndpoint($"{VirtualDirectory}/swagger/v1/swagger.json", $"kv.db v1");
                    c.RoutePrefix = VirtualDirectory;
                }
                else
                {
                    c.SwaggerEndpoint($"/swagger/v1/swagger.json", $"kv.db v1");
                    c.RoutePrefix = string.Empty;
                }
            });
        }
    }
}
