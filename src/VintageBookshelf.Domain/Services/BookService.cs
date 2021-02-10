using System.Threading.Tasks;
using VintageBookshelf.Domain.Interfaces;
using VintageBookshelf.Domain.Models;
using VintageBookshelf.Domain.Models.Validations;
using VintageBookshelf.Domain.Notifications;

namespace VintageBookshelf.Domain.Services
{
    public sealed class BookService : BaseService, IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository, 
                            INotifier notifier) : base(notifier)
        {
            _bookRepository = bookRepository;
        }
        
        public async Task Add(Book book)
        {
            if (!Validate(new BookValidator(), book))
            {
                return;
            }

            await _bookRepository.Add(book);
        }

        public async Task Update(Book book)
        {
            if (!Validate(new BookValidator(), book))
            {
                return;
            }

            await _bookRepository.Update(book);
        }

        public async Task Remove(long id)
        {
            await _bookRepository.Remove(id);
        }

        public void Dispose()
        {
            _bookRepository?.Dispose();
        }
    }
}