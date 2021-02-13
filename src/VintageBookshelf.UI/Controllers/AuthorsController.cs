using System.Collections.Generic;
using System.Linq;
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
    public class AuthorsController : BaseController
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IAuthorService _authorService;
        private readonly IMapper _mapper;

        public AuthorsController(IAuthorRepository authorRepository, 
                                 IAuthorService authorService, 
                                 IMapper mapper,
                                 INotifier notifier) : base(notifier)
        {
            _authorRepository = authorRepository;
            _authorService = authorService;
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
        public async Task<IActionResult> Create(AuthorViewModel authorViewModel)
        {
            if (ModelState.IsValid)
            {
                await _authorService.Add(_mapper.Map<Author>(authorViewModel));
                return RedirectToAction("Index");
            }
            
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
        public async Task<IActionResult> Edit(long id, AuthorViewModel authorViewModel)
        {
            if (id != authorViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _authorService.Update(_mapper.Map<Author>(authorViewModel));
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
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var authorViewModel = _mapper.Map<AuthorViewModel>(await _authorRepository.GetById(id));
            if (authorViewModel == null)
            {
                return NotFound();
            }

            await _authorService.Remove(id);
            return RedirectToAction("Index");
        }
    }
}
