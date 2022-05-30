using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace YTS.WEBAPI.CaseTest
{
    /// <summary>
    /// Web API ������
    /// </summary>
    public class Program : AbsBasicSimpleStartup
    {
        /// <summary>
        /// ʵ���� - ������
        /// </summary>
        /// <param name="conf">������</param>
        public Program(IConfiguration conf) : base(conf) { }

        /// <summary>
        /// ��ڳ���
        /// </summary>
        /// <param name="args">����</param>
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
