using System;
using System.IO;
using System.Reflection;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace YTS.IOFile.API.Swagger
{
    /// <summary>
    /// 启动项配置: Swagger API 文档
    /// </summary>
    public static class StartupSwaggerExtend
    {
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
                string app_basic_path = AppContext.BaseDirectory;
                Console.WriteLine($"ApplicationBasePath: [{app_basic_path}]");
                var name = Assembly.GetExecutingAssembly().GetName().Name;
                Console.WriteLine($"XmlCommentName: [{name}]");
                var xmlFile = $"{name}.xml";
                var xmlPath = Path.Combine(app_basic_path, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }
        /// <summary>
        /// 应用程序启用 Swagger API 文档浏览
        /// </summary>
        public static void StartEnableSwagger(this IApplicationBuilder app, IConfiguration conf)
        {
            string VirtualDirectory = conf.GetValue<string>("VirtualDirectory")
                ?.Trim()?.TrimEnd('/') ?? string.Empty;
            if (!string.IsNullOrEmpty(VirtualDirectory))
            {
                //app.UsePathBase(new PathString(VirtualDirectory));
                Console.WriteLine($"VirtualDirectory: [{VirtualDirectory}]");
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"{VirtualDirectory}/swagger/v1/swagger.json", $"kv.db v1");
                    //c.RoutePrefix = VirtualDirectory;
                    c.RoutePrefix = string.Empty;
                });
                return;
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", $"kv.db v1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
