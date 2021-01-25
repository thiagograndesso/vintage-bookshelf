using Microsoft.EntityFrameworkCore;
using VintageBookshelf.Domain.Models;

namespace VintageBookshelf.Data.Context
{
    public class VintageBookshelfContext : DbContext
    {
        public VintageBookshelfContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Library> Libraries { get; set; }

    }
}