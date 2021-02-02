using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VintageBookshelf.UI.ViewModels;

namespace VintageBookshelf.UI.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<VintageBookshelf.UI.ViewModels.AuthorViewModel> AuthorViewModel { get; set; }
        public DbSet<VintageBookshelf.UI.ViewModels.BookshelfViewModel> BookshelfViewModel { get; set; }
    }
}
