using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using YTS.WebApi;

namespace YTS.AdminWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // 自定义注入
            host.EnterHostDataBases();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            IHostBuilder builder = Host.CreateDefaultBuilder(args);
            return builder.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
        }
    }
}
