using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VintageBookshelf.Domain.Interfaces;
using VintageBookshelf.Domain.Models;
using VintageBookshelf.UI.ViewModels;
using static System.IO.File;

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
        
        [Route("list-books")]
        public async Task<IActionResult> Index()
        {
            var books = await _bookRepository.GetAllWithAuthorAndBookshelf();
            return View(_mapper.Map<IEnumerable<BookViewModel>>(books));
        }
        
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
        
        [Route("new-book")]
        public async Task<IActionResult> Create()
        {
            var bookViewModel = await PopulateAuthorsAndBookshelves(new BookViewModel());
            return View(bookViewModel);
        }
        
        [Route("new-book")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookViewModel bookViewModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(m => m.Errors).ToList();
                await PopulateAuthorsAndBookshelves(bookViewModel);
                return View(bookViewModel);
            }

            if (!await UploadImage(bookViewModel))
            {
                await PopulateAuthorsAndBookshelves(bookViewModel);
                return View(bookViewModel);
            }

            await _bookRepository.Add(_mapper.Map<Book>(bookViewModel));
            await _bookRepository.SaveChanges();
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
        
        [Route("edit-book")]
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                
                await _bookRepository.Update(_mapper.Map<Book>(bookViewModelUpdate));
                return RedirectToAction("Index");
            }
            
            return View(bookViewModel);
        }
        
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
        
        [Route("delete-book")]
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
            return RedirectToAction("Index");
        }

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

        [Route("update-book-author/{id:long}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAuthor(BookViewModel bookViewModel)
        {
            ModelState.Remove("Title");
            ModelState.Remove("Publisher");
            ModelState.Remove("Summary");
            
            if (!ModelState.IsValid)
            {
                return PartialView("_UpdateAuthor", bookViewModel);
            }

            await _authorRepository.Update(_mapper.Map<Author>(bookViewModel.Author));

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
