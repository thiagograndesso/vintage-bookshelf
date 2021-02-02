using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VintageBookshelf.Domain.Interfaces;
using VintageBookshelf.Domain.Models;
using VintageBookshelf.UI.ViewModels;

namespace VintageBookshelf.UI.Controllers
{
    public class BooksController : BaseController
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookshelfRepository _bookshelfRepository;
        private readonly IMapper _mapper;

        public BooksController(IBookRepository bookRepository, 
            IAuthorRepository authorRepository, 
            IBookshelfRepository bookshelfRepository, 
            IMapper mapper)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _bookshelfRepository = bookshelfRepository;
            _mapper = mapper;
        }
        
        public async Task<IActionResult> Index()
        {
            var books = await _bookRepository.GetAllWithAuthorAndBookshelf();
            return View(_mapper.Map<IEnumerable<BookViewModel>>(books));
        }
        
        public async Task<IActionResult> Details(long id)
        {
            var bookViewModel = _mapper.Map<BookViewModel>(await _bookRepository.GetBookWithAuthorAndBookshelf(id));
            if (bookViewModel == null)
            {
                return NotFound();
            }

            return View(bookViewModel);
        }
        
        public async Task<IActionResult> Create()
        {
            var bookViewModel = await PopulateAuthorsAndBookshelves(new BookViewModel());
            return View(bookViewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                await _bookRepository.Add(_mapper.Map<Book>(bookViewModel));
                await _bookRepository.SaveChanges();
                return RedirectToAction("Index");
            }

            await PopulateAuthorsAndBookshelves(bookViewModel);
            
            return View(bookViewModel);
        }
        
        public async Task<IActionResult> Edit(long id)
        {
            var bookViewModel = _mapper.Map<BookViewModel>(await _bookRepository.GetBookWithAuthorAndBookshelf(id));
            if (bookViewModel == null)
            {
                return NotFound();
            }

            await PopulateAuthorsAndBookshelves(bookViewModel);
            
            return View(bookViewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, BookViewModel bookViewModel)
        {
            if (id != bookViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _bookRepository.Update(_mapper.Map<Book>(bookViewModel));
                await _bookRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            
            await PopulateAuthorsAndBookshelves(bookViewModel);
            
            return View(bookViewModel);
        }
        
        public async Task<IActionResult> Delete(long id)
        {
            var bookViewModel = _mapper.Map<BookViewModel>(await _bookRepository.GetBookWithAuthorAndBookshelf(id));
            if (bookViewModel == null)
            {
                return NotFound();
            }

            return View(bookViewModel);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var bookViewModel = _mapper.Map<BookViewModel>(await _bookRepository.GetById(id));
            if (bookViewModel == null)
            {
                return NotFound();
            }
            
            await _bookRepository.Remove(id);
            await _bookRepository.SaveChanges();
            return RedirectToAction("Index");
        }

        private async Task<BookViewModel> PopulateAuthorsAndBookshelves(BookViewModel viewModel)
        {
            viewModel.Authors = _mapper.Map<IEnumerable<AuthorViewModel>>(await _authorRepository.GetAll());
            viewModel.Bookshelfs = _mapper.Map<IEnumerable<BookshelfViewModel>>(await _bookshelfRepository.GetAll());
            return viewModel;
        }
    }
}
