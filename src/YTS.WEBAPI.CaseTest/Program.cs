using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace YTS.WEBAPI.CaseTest
{
    /// <summary>
    /// Web API 启动项
    /// </summary>
    public class Program : AbsBasicSimpleStartup
    {
        /// <summary>
        /// 实例化 - 启动项
        /// </summary>
        /// <param name="conf">配置项</param>
        public Program(IConfiguration conf) : base(conf) { }

        /// <summary>
        /// 入口程序
        /// </summary>
        /// <param name="args">参数</param>
        public static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Program>();
                })
                .Build().Run();
        }
    }
}
