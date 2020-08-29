using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using YTS.Test;

namespace Test.ConsoleProgram
{
    public class Libray : ITestFactory
    {
        private ITestOutput output;
        private IServiceCollection services;
        public Libray(ITestOutput output)
        {
            this.output = output;
            this.services = new ServiceCollection();
            this.services.AddSingleton<ITestOutput>(s => output);
            Registered_IServiceLibray();
        }

        private void Registered_IServiceLibray()
        {
            services.AddScoped<IServiceLibray, Base.Libray>();
        }

        public IList<ITestItem> GetItems()
        {
            var provider = services.BuildServiceProvider();
            var libs = provider.GetServices<IServiceLibray>();
            foreach (var lib in libs)
            {
                lib.Registered(services);
            }
            provider = services.BuildServiceProvider();
            return provider.GetServices<ITestItem>().ToList();
        }
    }
}
