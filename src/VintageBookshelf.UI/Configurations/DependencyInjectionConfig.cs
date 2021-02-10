using Microsoft.Extensions.DependencyInjection;
using VintageBookshelf.Data.Context;
using VintageBookshelf.Data.Repository;
using VintageBookshelf.Domain.Interfaces;
using VintageBookshelf.Domain.Notifications;
using VintageBookshelf.Domain.Services;

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

            services.AddScoped<INotifier, Notifier>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IBookshelfService, BookshelfService>();
            services.AddScoped<IAuthorService, AuthorService>();
            
            return services;
        }
    }
}