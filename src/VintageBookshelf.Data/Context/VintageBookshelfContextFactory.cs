using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace VintageBookshelf.Data.Context
{
    public class VintageBookshelfContextFactory : IDesignTimeDbContextFactory<VintageBookshelfContext>
    {
        public VintageBookshelfContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<VintageBookshelfContext>();
            optionsBuilder.UseSqlite("Data Source=bookshelf2.db");

            return new VintageBookshelfContext(optionsBuilder.Options);
        }
    }
}