using VintageBookshelf.Data.Context;
using VintageBookshelf.Domain.Interfaces;
using VintageBookshelf.Domain.Models;

namespace VintageBookshelf.Data.Repository
{
    public sealed class BookshelfRepository : Repository<Bookshelf>, IBookshelfRepository
    {
        public BookshelfRepository(VintageBookshelfContext db) : base(db) { }
    }
}