using System;
using System.IO;
using System.Linq;

using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace YTS.WEBAPI.Swagger
{
    /// <summary>
    /// 启动项配置: Swagger API 文档
    /// </summary>
    public static class ServiceExtend
    {
        /// <summary>
        /// Swagger 节点名称
        /// </summary>
        public const string SwaggerEndpointName = "V1";
        /// <summary>
        /// Swagger 节点文档
        /// </summary>
        public const string SwaggerEndpointUrl = "/swagger/v1/swagger.json";

        /// <summary>
        /// 程序配置名称: Swagger信息名曾
        /// </summary>
        public const string APPSettingName_SwaggerInfo = "SwaggerInfo";

        /// <summary>
        /// 注入服务 Swagger API 浏览配置
        /// </summary>
        public static void EnterServiceSwagger(this IServiceCollection services, IConfiguration conf)
        {
            var swaggerInfo = conf.GetSection(APPSettingName_SwaggerInfo);
            var model = swaggerInfo.Get<SwaggerInfo>();
            if (model == null)
            {
                throw new NullReferenceException("请配置 SwaggerInfo 文档信息!");
            }

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
                SetCodeXml(c, model.ApiXmlName);
            });
        }

        private static void SetCodeXml(SwaggerGenOptions c, string ApiXmlName)
        {
            // Set the comments path for the Swagger JSON and UI.
            // 设置Swagger JSON和UI的注释路径。读取代码XML注释文档
            string app_basic_path = AppContext.BaseDirectory;
            DirectoryInfo rootdire = new DirectoryInfo(app_basic_path);
            var files = rootdire.GetFiles();
            var xml_files = files.Where(b => b.Extension.ToLower() == ".xml").ToArray();
            foreach (FileInfo file in xml_files)
            {
                c.IncludeXmlComments(file.FullName, true);
            }
        }

        /// <summary>
        /// 应用程序启用 Swagger API 文档浏览
        /// </summary>
        public static void StartEnableSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", $"v1");
                c.RoutePrefix = string.Empty;
            });
            app.SetDefaultHTML();
        }

        /// <summary>
        /// 应用程序启用 Swagger API 文档浏览, 使用二级目录, (未完成) 测试不好用
        /// </summary>
        [Obsolete("未完成, 测试未通过", false)]
        public static void StartEnableSwaggerVirtualDirectory(this IApplicationBuilder app, IConfiguration conf)
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
                    c.SwaggerEndpoint($"{VirtualDirectory}/swagger/v1/swagger.json", $"v1");
                    //c.RoutePrefix = VirtualDirectory;
                    c.RoutePrefix = string.Empty;
                });
                app.SetDefaultHTML();
                return;
            }
            app.StartEnableSwagger();
        }

        /// <summary>
        /// 配置默认页面
        /// </summary>
        public static void SetDefaultHTML(this IApplicationBuilder app)
        {
            DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
            defaultFilesOptions.DefaultFileNames.Clear();
            defaultFilesOptions.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(defaultFilesOptions);
            app.UseStaticFiles();
        }
    }
}
