using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VintageBookshelf.Domain.Interfaces;
using VintageBookshelf.Domain.Models;
using VintageBookshelf.Domain.Notifications;
using VintageBookshelf.Domain.Services;
using VintageBookshelf.UI.ViewModels;

namespace VintageBookshelf.UI.Controllers
{
    public class BookshelvesController : BaseController
    {
        private readonly IBookshelfRepository _bookshelfRepository;
        private readonly IBookshelfService _bookshelfService;
        private readonly IMapper _mapper;

        public BookshelvesController(IBookshelfRepository bookshelfRepository, 
                                     IBookshelfService bookshelfService, 
                                     IMapper mapper, 
                                     INotifier notifier) : base(notifier)
        {
            
            _bookshelfRepository = bookshelfRepository;
            _bookshelfService = bookshelfService;
            _mapper = mapper;
        }
        
        [Route("list-bookshelves")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<BookshelfViewModel>>(await _bookshelfRepository.GetAll()));
        }
        
        [Route("bookshelf-details/{id:long}")]
        public async Task<IActionResult> Details(long id)
        {
            var bookshelfViewModel = _mapper.Map<BookshelfViewModel>(await _bookshelfRepository.GetById(id));
            if (bookshelfViewModel == null)
            {
                return NotFound();
            }

            return View(bookshelfViewModel);
        }
        
        [Route("new-bookshelf")]
        public IActionResult Create()
        {
            return View();
        }
        
        [Route("new-bookshelf")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookshelfViewModel bookshelfViewModel)
        {
            if (ModelState.IsValid)
            {
                await _bookshelfService.Add(_mapper.Map<Bookshelf>(bookshelfViewModel));
                return RedirectToAction(nameof(Index));
            }
            
            return View(bookshelfViewModel);
        }

        [Route("edit-bookshelf/{id:long}")]
        public async Task<IActionResult> Edit(long id)
        {
            var bookshelfViewModel = _mapper.Map<BookshelfViewModel>(await _bookshelfRepository.GetById(id));
            if (bookshelfViewModel == null)
            {
                return NotFound();
            }
            return View(bookshelfViewModel);
        }
        
        [Route("edit-bookshelf/{id:long}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, BookshelfViewModel bookshelfViewModel)
        {
            if (ModelState.IsValid)
            {
                await _bookshelfService.Update(_mapper.Map<Bookshelf>(bookshelfViewModel));
                return RedirectToAction(nameof(Index));
            }
            
            return View(bookshelfViewModel);
        }
        
        [Route("delete-bookshelf/{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var bookshelfViewModel = _mapper.Map<BookshelfViewModel>(await _bookshelfRepository.GetById(id));
            if (bookshelfViewModel == null)
            {
                return NotFound();
            }

            return View(bookshelfViewModel);
        }
        
        [Route("delete-bookshelf/{id:long}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var bookshelfViewModel = _mapper.Map<BookshelfViewModel>(await _bookshelfRepository.GetById(id));
            if (bookshelfViewModel == null)
            {
                return NotFound();
            }
            await _bookshelfService.Remove(id);
            return RedirectToAction("Index");
        }
    }
}
