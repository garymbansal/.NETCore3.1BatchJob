
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Batch.AppSettings;
using Batch.Services;
using Serilog;
namespace Batch
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var services = LoadServices();
                var serviceProvider = services.BuildServiceProvider(true);
                serviceProvider.GetService<App>().Start();

            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Error in App: {0}", ex.Message);
            }
        }
        private static IServiceCollection LoadServices()
        {
            var services = new ServiceCollection();

            var applogger = new LoggerConfiguration()
                                .WriteTo.File("batch.log")
                                .CreateLogger();
            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Information);
                builder.AddSerilog(logger: applogger, dispose: true);
            });

            services.AddSingleton<IAppSetting, AppSetting>();
            services.AddSingleton<IDataService, DataService>();
            services.AddTransient<App>();
            return services;
        }
    }
}
