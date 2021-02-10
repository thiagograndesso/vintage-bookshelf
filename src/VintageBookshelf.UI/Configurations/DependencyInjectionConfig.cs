using Microsoft.Extensions.DependencyInjection;
using VintageBookshelf.Data.Context;
using VintageBookshelf.Data.Repository;
using VintageBookshelf.Domain.Interfaces;

namespace VintageBookshelf.UI.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<VintageBookshelfContext>()
                .AddScoped<IBookRepository, BookRepository>()
                .AddScoped<IAuthorRepository, AuthorRepository>()
                .AddScoped<IBookshelfRepository, BookshelfRepository>();
            
            return services;
        }
    }
}