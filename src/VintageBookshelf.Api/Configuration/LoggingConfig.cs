using System;
using Elmah.Io.AspNetCore;
using Elmah.Io.AspNetCore.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VintageBookshelf.Api.Extensions;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace VintageBookshelf.Api.Configuration
{
    public static class LoggingConfig
    {
        public static IServiceCollection AddLoggingConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration
                .GetSection("Elmah")
                .Get<ElmahSettings>();
            
            services.AddElmahIo(o =>
            {
                o.ApiKey = settings.ApiKey;
                o.LogId = new Guid(settings.LogId);
            });

            services.AddHealthChecks()
                    .AddElmahIoPublisher(o =>
                    {
                        o.ApiKey = settings.ApiKey;
                        o.LogId = new Guid(settings.LogId);
                    })
                    .AddSqlServer(configuration.GetConnectionString("DefaultConnection"), name: "Database check");

            services.AddHealthChecksUI()
                    .AddInMemoryStorage();
            
            return services;
        }

        public static IApplicationBuilder UseLoggingConfig(this IApplicationBuilder app)
        {
            app.UseElmahIo();

            app.UseHealthChecks("/api/health", new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecksUI();

            return app;
        }
    }
}