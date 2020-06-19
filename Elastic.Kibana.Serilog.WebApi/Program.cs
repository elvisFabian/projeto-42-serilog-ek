using Elastic.Kibana.Serilog.ExtensionsMethods;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Elastic.Kibana.Serilog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var configuration = host.Services.GetService<IConfiguration>();
            Log.Logger = CustomLoggerFactory.CreateLogger(configuration);

            host.Run();
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((context, builder) => { builder.AddSerilog(dispose: true); })
                .ConfigureWebHostDefaults(builder =>
                {
                    builder
                        .UseStartup<Startup>()
                        .UseSerilog();
                });
    }
}