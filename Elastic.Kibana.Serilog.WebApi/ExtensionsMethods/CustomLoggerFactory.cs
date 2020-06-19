using System;
using System.Reflection;
using Destructurama;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;

namespace Elastic.Kibana.Serilog.ExtensionsMethods
{
    public static class CustomLoggerFactory
    {
        public static Logger CreateLogger(IConfiguration configuration)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var logger = new LoggerConfiguration()
                .Enrich.WithThreadId()
                .Enrich.WithThreadName()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithProcessId()
                .Enrich.WithProcessName()
                .Enrich.WithProperty("Environment", environment)
                .Destructure.UsingAttributes()
                .ReadFrom.Configuration(configuration)
                .MinimumLevel.Override("IdentityServer4.AccessTokenValidation.IdentityServerAuthenticationHandler", LogEventLevel.Error)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.ChangeTracking", LogEventLevel.Error)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.ChangeTracking", LogEventLevel.Error)
                .MinimumLevel.Override("Microsoft.AspNetCore.Server.Kestrel", LogEventLevel.Error)
                .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
                .WriteTo.Console()
                .WriteTo.Elasticsearch(ConfigureElasticSink(environment, configuration))
                .CreateLogger();

            return logger;
        }

        private static ElasticsearchSinkOptions ConfigureElasticSink(string environment, IConfiguration configuration)
        {
            var indexName = Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-");
            var environmentName = environment?.ToLower().Replace(".", "-");
            var node = new Uri(configuration["ElasticConfiguration:Uri"]);

            return new ElasticsearchSinkOptions(node)
            {
                AutoRegisterTemplate = false, //O coletor registrará um modelo de índice para os logs na elasticsearch
                IndexFormat = $"{indexName}-{environmentName}-{DateTime.UtcNow:yyyy-MM-dd}",
                Period = TimeSpan.FromSeconds(1), //Intervalo de sincronização. 2 segundos é o padrão
                BatchPostingLimit = 50, //Número máximo de eventos no post
                QueueSizeLimit = 100000 //Número maximo de eventos mantidos em memória até sincronizar com o elastic. Além desse limite os eventos serão descartados  
            };
        }
    }
}