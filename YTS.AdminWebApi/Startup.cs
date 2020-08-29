using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using YTS.WebApi;

namespace YTS.AdminWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// 运行时将调用此方法。 使用此方法将服务添加到容器。
        /// </summary>
        /// <param name="services">服务</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // 自定义注入服务
            services.EnterServiceControllers();
            services.EnterServiceJson();
            services.EnterServiceCors();
            services.EnterServiceSwagger(Configuration);
            services.EnterServiceJWTAuthentication(Configuration);
            services.EnterServiceDataBases(Configuration);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// 运行时将调用此方法。 使用此方法来配置HTTP请求管道。
        /// </summary>
        /// <param name="app">应用程序生成器</param>
        /// <param name="env">IWebHost环境</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            // 自定义配置启用
            app.StartEnableRoute();
            app.StartEnableCors();
            app.StartEnableSwagger();

            app.UseEndpoints(endpoints =>
            {
                var builder = endpoints.MapControllers();
            });
        }
    }
}
