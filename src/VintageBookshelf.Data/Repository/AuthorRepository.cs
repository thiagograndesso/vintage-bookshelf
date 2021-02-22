using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VintageBookshelf.Data.Context;
using VintageBookshelf.Domain.Interfaces;
using VintageBookshelf.Domain.Models;
using VintageBookshelf.Domain.Services;

namespace VintageBookshelf.Data.Repository
{
    public sealed class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        public AuthorRepository(VintageBookshelfContext db) : base(db) { }

        public async Task<Author> GetAuthorWithBooks(long id)
        {
            return await Db.Authors
                .AsNoTracking()
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
        
        public async Task<IEnumerable<Author>> GetAllWithBooks()
        {
            return await Db.Authors
                .AsNoTracking()
                .Include(a => a.Books)
                .ToListAsync();
        }
    }
}