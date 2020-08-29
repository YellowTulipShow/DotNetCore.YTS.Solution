using Microsoft.Extensions.DependencyInjection;

namespace Test.ConsoleProgram.Base
{
    public class Libray : IServiceLibray
    {
        public void Registered(IServiceCollection services)
        {
            services.AddTest<Test_ConvertTool_ToDecimal>();
            services.AddTest<Test_FilePath>();
        }
    }
}
