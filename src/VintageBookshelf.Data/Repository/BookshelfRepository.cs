using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VintageBookshelf.Data.Context;
using VintageBookshelf.Domain.Interfaces;
using VintageBookshelf.Domain.Models;

namespace VintageBookshelf.Data.Repository
{
    public sealed class BookshelfRepository : Repository<Bookshelf>, IBookshelfRepository
    {
        public BookshelfRepository(VintageBookshelfContext db) : base(db) { }
        public async Task<Bookshelf> GetBookshelfWithProducts(long id)
        {
            return await Db.Bookshelves
                .AsNoTracking()
                .Include(b => b.Books)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Bookshelf>> GetAllWithProducts()
        {
            return await Db.Bookshelves
                .AsNoTracking()
                .Include(b => b.Books)
                .ToListAsync();
        }
        
        
    }
}