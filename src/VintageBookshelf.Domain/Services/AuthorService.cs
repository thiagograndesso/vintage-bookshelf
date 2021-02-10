using System.Threading.Tasks;
using VintageBookshelf.Domain.Interfaces;
using VintageBookshelf.Domain.Models;
using VintageBookshelf.Domain.Models.Validations;
using VintageBookshelf.Domain.Notifications;

namespace VintageBookshelf.Domain.Services
{
    public sealed class AuthorService : BaseService, IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository, INotifier notifier) : base(notifier)
        {
            _authorRepository = authorRepository;
        }
        
        public async Task Add(Author author)
        {
            if (!Validate(new AuthorValidator(), author))
            {
                return;
            }

            await _authorRepository.Add(author);
        }

        public async Task Update(Author author)
        {
            if (!Validate(new AuthorValidator(), author))
            {
                return;
            }

            await _authorRepository.Update(author);
        }

        public async Task Remove(long id)
        {
            await _authorRepository.Remove(id);
        }

        public void Dispose()
        {
            _authorRepository?.Dispose();
        }
    }
}