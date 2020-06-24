using System.Collections.Generic;
using System.IO;
using Elastic.Kibana.Serilog.Dapper;
using Elastic.Kibana.Serilog.EF;
using Elastic.Kibana.Serilog.ExtensionsMethods;
using Elastic.Kibana.Serilog.Middleware;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Elastic.Kibana.Serilog
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddAuthorization();

            var projeto42ConnectionString = Configuration.GetConnectionString("Projeto42");

            var connectionDict = new Dictionary<DatabaseConnectionName, string>
            {
                {DatabaseConnectionName.Projeto42, projeto42ConnectionString},
            };

            services.AddSingleton<IDictionary<DatabaseConnectionName, string>>(connectionDict);
            services.AddTransient<IDbConnectionFactory, DapperDbConnectionFactory>();
            services.AddTransient<IPessoaRepository, PessoaRepository>();

            services.AddHttpContextAccessor();

            services.AddDbContext<Projeto42DbContext>(options => { options.UseSqlServer(projeto42ConnectionString); });

            services
                .AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Configuration["Idp:Authority"];
                    options.ApiName = Configuration["Idp:ApiName"];
                    options.RequireHttpsMetadata = false;
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IHostApplicationLifetime hostApplicationLifetime)
        {
            loggerFactory.AddSerilog(Log.Logger);

            //HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms
            app.UseSerilogRequestLogging(opt =>
            {
                opt.EnrichDiagnosticContext = async (diagnosticContext, httpContext) =>
                {
                    var user = new LogUserProperties(httpContext);

                    diagnosticContext.Set(LogConstantes.user_id, user.UserId);
                    diagnosticContext.Set(LogConstantes.user_name, user.Name);
                    diagnosticContext.Set(LogConstantes.user_email, user.Email);
                };
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseHttpsRedirection()
                .UseRouting()
                .UseAuthentication();

            app.UseAuthorization();

            app
                .UseMiddleware<ErrorHandlingMiddleware>()
                .UseEndpoints(endpoints => { endpoints.MapControllers(); });


            hostApplicationLifetime.ApplicationStopped.Register(Log.CloseAndFlush);
        }
    }
}