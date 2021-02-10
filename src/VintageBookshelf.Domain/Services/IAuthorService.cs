using System;
using System.Threading.Tasks;
using VintageBookshelf.Domain.Models;

namespace VintageBookshelf.Domain.Services
{
    public interface IAuthorService : IDisposable
    {
        Task Add(Author author);
        Task Update(Author author);
        Task Remove(long id);
    }
}