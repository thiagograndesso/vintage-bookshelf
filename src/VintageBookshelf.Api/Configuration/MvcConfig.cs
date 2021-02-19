using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace VintageBookshelf.Api.Configuration
{
    public static class MvcConfig
    {
        public static IServiceCollection AddMvc(this IServiceCollection services)
        {
            services.AddControllers();
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
            
            return services;
        }
    }
}