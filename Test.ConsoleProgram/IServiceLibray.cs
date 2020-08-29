using Microsoft.Extensions.DependencyInjection;
using YTS.Test;

namespace Test.ConsoleProgram
{
    public interface IServiceLibray
    {
        void Registered(IServiceCollection services);
    }

    public static class IServiceLibrayExtend
    {
        public static void AddTest<T>(this IServiceCollection services) where T : class, ITestItem
        {
            services.AddScoped<ITestItem, T>();
        }
    }
}
