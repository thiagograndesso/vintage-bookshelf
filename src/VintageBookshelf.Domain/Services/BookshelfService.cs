using System.Threading.Tasks;
using VintageBookshelf.Domain.Interfaces;
using VintageBookshelf.Domain.Models;
using VintageBookshelf.Domain.Models.Validations;
using VintageBookshelf.Domain.Notifications;

namespace VintageBookshelf.Domain.Services
{
    public sealed class BookshelfService : BaseService, IBookshelfService
    {
        private readonly IBookshelfRepository _bookshelfRepository;

        public BookshelfService(IBookshelfRepository bookshelfRepository, INotifier notifier) : base(notifier)
        {
            _bookshelfRepository = bookshelfRepository;
        }
        
        public async Task Add(Bookshelf bookshelf)
        {
            if (!Validate(new BookshelfValidator(), bookshelf))
            {
                return;
            }

            await _bookshelfRepository.Add(bookshelf);
        }

        public async Task Update(Bookshelf bookshelf)
        {
            if (!Validate(new BookshelfValidator(), bookshelf))
            {
                return;
            }

            await _bookshelfRepository.Update(bookshelf);
        }

        public async Task Remove(long id)
        {
            await _bookshelfRepository.Remove(id);
        }

        public void Dispose()
        {
            _bookshelfRepository?.Dispose();
        }
    }
}