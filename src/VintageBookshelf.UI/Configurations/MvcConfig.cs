using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace VintageBookshelf.UI.Configurations
{
    public static class MvcConfig
    {
        public static IServiceCollection AddMvcConfiguration(this IServiceCollection services)
        {
            services.AddControllersWithViews(o =>
                o.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));
            
            return services;
        }
    }
}