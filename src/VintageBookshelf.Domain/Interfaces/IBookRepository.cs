using System.Threading.Tasks;
using VintageBookshelf.Domain.Models;

namespace VintageBookshelf.Domain.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<Book> GetBookWithAuthor(long id);
        Task<Book> GetBookWithBookshelf(long id);
    }
}