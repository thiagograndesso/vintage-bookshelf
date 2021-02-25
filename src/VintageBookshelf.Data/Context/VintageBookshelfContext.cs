using System.Linq;
using Microsoft.EntityFrameworkCore;
using VintageBookshelf.Domain.Models;

namespace VintageBookshelf.Data.Context
{
    public class VintageBookshelfContext : DbContext
    {
        public VintageBookshelfContext(DbContextOptions options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Bookshelf> Bookshelves { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(VintageBookshelfContext).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
            }
            
            base.OnModelCreating(modelBuilder);
        }
    } 
}