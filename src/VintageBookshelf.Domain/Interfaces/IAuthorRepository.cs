using System.Collections.Generic;
using System.Threading.Tasks;
using VintageBookshelf.Domain.Models;

namespace VintageBookshelf.Domain.Interfaces
{
    public interface IAuthorRepository : IRepository<Author>
    {
        Task<Author> GetAuthorWithBooks(long id);
        Task<IEnumerable<Author>> GetAllWithBooks();
    }
}