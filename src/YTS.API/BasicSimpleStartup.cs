using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using YTS.WEBAPI.Cors;
using YTS.WEBAPI.Swagger;

namespace YTS.WEBAPI
{
    /// <summary>
    /// ������ļ򵥿�ʼ����
    /// </summary>
    public class BasicSimpleStartup
    {
        /// <summary>
        /// ������
        /// </summary>
        protected readonly IConfiguration conf;

        /// <summary>
        /// ʵ������ʼ����
        /// </summary>
        /// <param name="conf"></param>
        public BasicSimpleStartup(IConfiguration conf)
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

            // �Զ�����������
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
