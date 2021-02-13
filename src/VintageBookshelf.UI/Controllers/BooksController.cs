using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VintageBookshelf.Domain.Interfaces;
using VintageBookshelf.Domain.Models;
using VintageBookshelf.Domain.Notifications;
using VintageBookshelf.Domain.Services;
using VintageBookshelf.UI.Extensions;
using VintageBookshelf.UI.ViewModels;
using static System.IO.File;

namespace VintageBookshelf.UI.Controllers
{
    [Authorize]
    public class BooksController : BaseController
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IBookshelfRepository _bookshelfRepository;
        private readonly IBookService _bookService;
        private readonly IAuthorService _authorService;
        private readonly IMapper _mapper;

        public BooksController(IBookRepository bookRepository, 
                               IAuthorRepository authorRepository, 
                               IBookshelfRepository bookshelfRepository, 
                               IBookService bookService,
                               IAuthorService authorService,
                               IMapper mapper,
                               INotifier notifier) : base(notifier)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _bookshelfRepository = bookshelfRepository;
            _bookService = bookService;
            _authorService = authorService;
            _mapper = mapper;
        }
        
        [AllowAnonymous]
        [Route("list-books")]
        public async Task<IActionResult> Index()
        {
            var books = await _bookRepository.GetAllWithAuthorAndBookshelf();
            return View(_mapper.Map<IEnumerable<BookViewModel>>(books));
        }
        
        [AllowAnonymous]
        [Route("book-details/{id:long}")]
        public async Task<IActionResult> Details(long id)
        {
            var bookViewModel = _mapper.Map<BookViewModel>(await _bookRepository.GetBookWithAuthorAndBookshelf(id));
            if (bookViewModel == null)
            {
                return NotFound();
            }

            return View(bookViewModel);
        }
        
        [ClaimsAuthorize("Book", "Add")]
        [Route("new-book")]
        public async Task<IActionResult> Create()
        {
            var bookViewModel = await PopulateAuthorsAndBookshelves(new BookViewModel());
            return View(bookViewModel);
        }
        
        [ClaimsAuthorize("Book", "Add")]
        [Route("new-book")]
        [HttpPost]
        public async Task<IActionResult> Create(BookViewModel bookViewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopulateAuthorsAndBookshelves(bookViewModel);
                return View(bookViewModel);
            }

            if (!await UploadImage(bookViewModel))
            {
                await PopulateAuthorsAndBookshelves(bookViewModel);
                return View(bookViewModel);
            }

            await _bookService.Add(_mapper.Map<Book>(bookViewModel));

            if (!IsOperationValid())
            {
                return View(bookViewModel);
            }
            
            return RedirectToAction("Index");
        }

        private async Task<bool> UploadImage(BookViewModel bookViewModel)
        {
            if (bookViewModel.UploadImage.Length <= 0)
            {
                return false;
            }
            
            var imageName = $"{Guid.NewGuid()}_{bookViewModel.UploadImage.FileName}";
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageName);

            if (Exists(imagePath))
            {
                ModelState.AddModelError(string.Empty,"File already exists!");
                return false;
            }

            await using var stream = new FileStream(imagePath, FileMode.Create);
            await bookViewModel.UploadImage.CopyToAsync(stream);

            bookViewModel.Image = imageName;
            
            return true;
        }

        [ClaimsAuthorize("Book", "Edit")]
        [Route("edit-book/{id:long}")]
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
        
        [ClaimsAuthorize("Book", "Edit")]
        [Route("edit-book")]
        [HttpPost]
        public async Task<IActionResult> Edit(long id, BookViewModel bookViewModel)
        {
            if (id != bookViewModel.Id)
            {
                return NotFound();
            }

            var bookViewModelUpdate = _mapper.Map<BookViewModel>(await _bookRepository.GetBookWithAuthorAndBookshelf(id));
            bookViewModel.Author = bookViewModelUpdate.Author;
            bookViewModel.Bookshelf = bookViewModel.Bookshelf;
            
            if (ModelState.IsValid)
            {
                if (bookViewModel.UploadImage is not null)
                {
                    if (!await UploadImage(bookViewModel))
                    {
                        return View(bookViewModel);
                    }

                    bookViewModelUpdate.Image = bookViewModel.Image;
                }

                bookViewModelUpdate.Title = bookViewModel.Title;
                bookViewModelUpdate.Summary = bookViewModel.Summary;
                bookViewModelUpdate.ReleaseYear = bookViewModel.ReleaseYear;
                bookViewModelUpdate.Publisher = bookViewModel.Publisher;
                
                await _bookService.Update(_mapper.Map<Book>(bookViewModelUpdate));
                
                if (!IsOperationValid())
                {
                    return View(bookViewModel);
                }
                
                return RedirectToAction("Index");
            }
            
            return View(bookViewModel);
        }
        
        [ClaimsAuthorize("Book", "Delete")]
        [Route("delete-book/{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var bookViewModel = _mapper.Map<BookViewModel>(await _bookRepository.GetBookWithAuthorAndBookshelf(id));
            if (bookViewModel == null)
            {
                return NotFound();
            }

            return View(bookViewModel);
        }
        
        [ClaimsAuthorize("Book", "Delete")]
        [Route("delete-book")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var bookViewModel = _mapper.Map<BookViewModel>(await _bookRepository.GetById(id));
            if (bookViewModel == null)
            {
                return NotFound();
            }
            
            await _bookService.Remove(id);
            
            if (!IsOperationValid())
            {
                return View(bookViewModel);
            }
            
            return RedirectToAction("Index");
        }

        [ClaimsAuthorize("Book", "Edit")]
        [Route("update-book-author/{id:long}")]
        public async Task<IActionResult> UpdateAuthor(long id)
        {
            var bookViewModel = _mapper.Map<BookViewModel>(await _bookRepository.GetBookWithAuthorAndBookshelf(id));
            if (bookViewModel == null)
            {
                return NotFound();
            }

            return PartialView("_UpdateAuthor", new BookViewModel { Author = bookViewModel.Author });
        }

        [AllowAnonymous]
        [Route("fetch-book-author/{id:long}")]
        public async Task<IActionResult> FetchAuthor(long id)
        {
            var bookViewModel = _mapper.Map<BookViewModel>(await _bookRepository.GetBookWithAuthor(id));
            if (bookViewModel == null)
            {
                return NotFound();
            }

            return PartialView("_AuthorDetails", bookViewModel);
        }

        [ClaimsAuthorize("Book", "Edit")]
        [Route("update-book-author/{id:long}")]
        [HttpPost]
        public async Task<IActionResult> UpdateAuthor(BookViewModel bookViewModel)
        {
            ModelState.Remove("Title");
            ModelState.Remove("Publisher");
            ModelState.Remove("Summary");
            
            if (!ModelState.IsValid)
            {
                return PartialView("_UpdateAuthor", bookViewModel);
            }

            await _authorService.Update(_mapper.Map<Author>(bookViewModel.Author));

            var url = Url.Action("FetchAuthor", "Books", new {id = bookViewModel.Id});
            return Json(new { success = true, url });
        }

        private async Task<BookViewModel> PopulateAuthorsAndBookshelves(BookViewModel viewModel)
        {
            viewModel.Authors = _mapper.Map<IEnumerable<AuthorViewModel>>(await _authorRepository.GetAll());
            viewModel.Bookshelves = _mapper.Map<IEnumerable<BookshelfViewModel>>(await _bookshelfRepository.GetAll());
            return viewModel;
        }
    }
}
