using VintageBookshelf.Data.Context;
using VintageBookshelf.Domain.Interfaces;
using VintageBookshelf.Domain.Models;

namespace VintageBookshelf.Data.Repository
{
    public sealed class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        public AuthorRepository(VintageBookshelfContext db) : base(db) { }
    }
}