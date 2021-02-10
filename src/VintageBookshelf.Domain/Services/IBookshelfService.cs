using System;
using System.Threading.Tasks;
using VintageBookshelf.Domain.Models;

namespace VintageBookshelf.Domain.Services
{
    public interface IBookshelfService : IDisposable
    {
        Task Add(Bookshelf bookshelf);
        Task Update(Bookshelf bookshelf);
        Task Remove(long id);
    }
}