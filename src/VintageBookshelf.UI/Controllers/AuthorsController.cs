using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VintageBookshelf.Domain.Interfaces;
using VintageBookshelf.Domain.Models;
using VintageBookshelf.UI.ViewModels;

namespace VintageBookshelf.UI.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public AuthorsController(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        [Route("list-authors")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<AuthorViewModel>>(await _authorRepository.GetAll()));
        }
        
        [Route("author-details/{id:long}")]
        public async Task<IActionResult> Details(long id)
        {
            var authorViewModel = _mapper.Map<AuthorViewModel>(await _authorRepository.GetById(id));
            if (authorViewModel == null)
            {
                return NotFound();
            }

            return View(authorViewModel);
        }
        
        [Route("new-author")]
        public IActionResult Create()
        {
            return View();
        }
        
        [Route("new-author")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuthorViewModel authorViewModel)
        {
            if (ModelState.IsValid)
            {
                await _authorRepository.Add(_mapper.Map<Author>(authorViewModel));
                await _authorRepository.SaveChanges();
                return RedirectToAction("Index");
            }

            var errors = ModelState.Values.SelectMany(m => m.Errors);
            
            return View(authorViewModel);
        }
        
        [Route("edit-author/{id:long}")]
        public async Task<IActionResult> Edit(long id)
        {
            var authorViewModel = _mapper.Map<AuthorViewModel>(await _authorRepository.GetById(id));
            if (authorViewModel == null)
            {
                return NotFound();
            }
            return View(authorViewModel);
        }
        
        [Route("edit-author/{id:long}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, AuthorViewModel authorViewModel)
        {
            if (id != authorViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _authorRepository.Update(_mapper.Map<Author>(authorViewModel));
                await _authorRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(authorViewModel);
        }
        
        [Route("delete-author/{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var authorViewModel = _mapper.Map<AuthorViewModel>(await _authorRepository.GetById(id));
            if (authorViewModel == null)
            {
                return NotFound();
            }

            return View(authorViewModel);
        }
        
        [Route("delete-author/{id:long}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var authorViewModel = _mapper.Map<AuthorViewModel>(await _authorRepository.GetById(id));
            if (authorViewModel == null)
            {
                return NotFound();
            }

            await _authorRepository.Remove(id);
            await _authorRepository.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
