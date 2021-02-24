using System.Threading.Tasks;
using VintageBookshelf.Domain.Models;

namespace VintageBookshelf.Domain.Interfaces
{
    public interface IBookshelfRepository : IRepository<Bookshelf>
    {
        Task<Bookshelf> GetBookshelfWithProducts(long id);
    }
}