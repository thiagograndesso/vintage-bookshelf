using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VintageBookshelf.Data.Context;
using VintageBookshelf.Domain.Interfaces;
using VintageBookshelf.Domain.Models;

namespace VintageBookshelf.Data.Repository
{
    public sealed class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(VintageBookshelfContext db) : base(db) { }
        
        public async Task<Book> GetBookWithAuthor(long id)
        {
            return await Db.Books.AsNoTracking()
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Book> GetBookWithBookshelf(long id)
        {
            return await Db.Books.AsNoTracking()
                .Include(b => b.Bookshelf)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Book> GetBookWithAuthorAndBookshelf(long id)
        {
            return await Db.Books.AsNoTracking()
                .Include(b => b.Author)
                .Include(b => b.Bookshelf)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Book>> GetAllWithAuthorAndBookshelf()
        {
            return await Db.Books.AsNoTracking()
                .Include(b => b.Author)
                .Include(b => b.Bookshelf)
                .ToListAsync();
        }
    }
}