using System.Collections.Generic;
using System.Threading.Tasks;
using VintageBookshelf.Domain.Models;

namespace VintageBookshelf.Domain.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<Book> GetBookWithAuthor(long id);
        Task<Book> GetBookWithBookshelf(long id);
        Task<Book> GetBookWithAuthorAndBookshelf(long id);
        Task<IEnumerable<Book>> GetAllWithAuthorAndBookshelf();
    }
}