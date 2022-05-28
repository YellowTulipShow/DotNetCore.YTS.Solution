using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using YTS.IOFile.API.Swagger;

namespace YTS.IOFile.API
{
    /// <summary>
    /// ��ʼ����
    /// </summary>
    public class Startup
    {
        private readonly IConfiguration conf;

        /// <summary>
        /// ʵ������ʼ����
        /// </summary>
        /// <param name="conf"></param>
        public Startup(IConfiguration conf)
        {
            this.conf = conf;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// ����ʱ�����ô˷����� ʹ�ô˷�����������ӵ�������
        /// </summary>
        /// <param name="services">����</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // �Զ���ע�����
            services.EnterServiceControllers();
            services.EnterServiceJson();
            services.EnterServiceCors();
            services.EnterServiceSwagger(conf);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// ����ʱ�����ô˷����� ʹ�ô˷���������HTTP����ܵ���
        /// </summary>
        /// <param name="app">Ӧ�ó���������</param>
        /// <param name="env">IWebHost����</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            // app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();

            // �Զ�����������
            app.StartEnableRoute();
            app.StartEnableCors();
            app.StartEnableSwagger(conf);

            app.UseEndpoints(endpoints =>
            {
                var builder = endpoints.MapControllers();
            });
        }
    }
}
