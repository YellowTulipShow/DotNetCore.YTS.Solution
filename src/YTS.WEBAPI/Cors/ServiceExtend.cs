using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace YTS.WEBAPI.Cors
{
    /// <summary>
    /// 跨域相关扩展服务配置
    /// </summary>
    public static class ServiceExtend
    {
        /// <summary>
        /// 跨域名称
        /// </summary>
        public const string CorsName = "GeneralGlobalAllowSpecificOrigins";

        /// <summary>
        /// 注入服务 跨域配置
        /// </summary>
        public static void EnterServiceCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(CorsName, builder =>
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
            app.UseCors(CorsName);
        }
    }
}
