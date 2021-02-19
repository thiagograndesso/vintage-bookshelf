using Microsoft.Extensions.DependencyInjection;
using VintageBookshelf.Data.Context;
using VintageBookshelf.Data.Repository;
using VintageBookshelf.Domain.Interfaces;
using VintageBookshelf.Domain.Notifications;
using VintageBookshelf.Domain.Services;

namespace VintageBookshelf.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<VintageBookshelfContext>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IBookshelfRepository, BookshelfRepository>();
            
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IBookshelfRepository, BookshelfRepository>();
            services.AddScoped<INotifier, Notifier>();
            
            return services;
        }
    }
}