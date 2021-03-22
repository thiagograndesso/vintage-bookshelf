using System;
using AutoMapper.Configuration;
using Elmah.Io.AspNetCore;
using Microsoft.AspNetCore.Builder;
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
                .GetSection("LoggingSettings:Elmah")
                .Get<ElmahSettings>();
            
            services.AddElmahIo(o =>
            {
                o.ApiKey = settings.ApiKey;
                o.LogId = new Guid(settings.LogId);
            });
            
            return services;
        }

        public static IApplicationBuilder UseLoggingConfig(this IApplicationBuilder app)
        {
            app.UseElmahIo();
            
            return app;
        }
    }
}