using System;
using System.Threading.Tasks;
using VintageBookshelf.Domain.Models;

namespace VintageBookshelf.Domain.Services
{
    public interface IBookService : IDisposable
    {
        Task Add(Book book);
        Task Update(Book book);
        Task Remove(long id);
    }
}