using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VintageBookshelf.Data.Repository;
using VintageBookshelf.Domain.Models;
using VintageBookshelf.UI.Data;
using VintageBookshelf.UI.ViewModels;

namespace VintageBookshelf.UI.Controllers
{
    public class BookshelvesController : BaseController
    {
        private readonly BookshelfRepository _bookshelfRepository;
        private readonly IMapper _mapper;

        public BookshelvesController(BookshelfRepository bookshelfRepository, IMapper mapper)
        {
            _bookshelfRepository = bookshelfRepository;
            _mapper = mapper;
        }
        
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<BookshelfViewModel>>(await _bookshelfRepository.GetAll()));
        }
        
        public async Task<IActionResult> Details(long id)
        {
            var bookshelfViewModel = _mapper.Map<BookshelfViewModel>(await _bookshelfRepository.GetById(id));
            if (bookshelfViewModel == null)
            {
                return NotFound();
            }

            return View(bookshelfViewModel);
        }
        
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookshelfViewModel bookshelfViewModel)
        {
            if (ModelState.IsValid)
            {
                await _bookshelfRepository.Add(_mapper.Map<Bookshelf>(bookshelfViewModel));
                await _bookshelfRepository.SaveChanges();
                
                return RedirectToAction(nameof(Index));
            }
            
            return View(bookshelfViewModel);
        }

        public async Task<IActionResult> Edit(long id)
        {
            var bookshelfViewModel = _mapper.Map<BookshelfViewModel>(await _bookshelfRepository.GetById(id));
            if (bookshelfViewModel == null)
            {
                return NotFound();
            }
            return View(bookshelfViewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, BookshelfViewModel bookshelfViewModel)
        {
            if (ModelState.IsValid)
            {
                await _bookshelfRepository.Update(_mapper.Map<Bookshelf>(bookshelfViewModel));
                await _bookshelfRepository.SaveChanges();
                
                return RedirectToAction(nameof(Index));
            }
            
            return View(bookshelfViewModel);
        }
        
        public async Task<IActionResult> Delete(long id)
        {
            var bookshelfViewModel = _mapper.Map<BookshelfViewModel>(await _bookshelfRepository.GetById(id));
            if (bookshelfViewModel == null)
            {
                return NotFound();
            }

            return View(bookshelfViewModel);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var bookshelfViewModel = _mapper.Map<BookshelfViewModel>(await _bookshelfRepository.GetById(id));
            if (bookshelfViewModel == null)
            {
                return NotFound();
            }
            await _bookshelfRepository.Remove(id);
            await _bookshelfRepository.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
