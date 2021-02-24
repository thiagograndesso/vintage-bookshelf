using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VintageBookshelf.Api.Dtos;
using VintageBookshelf.Domain.Interfaces;
using VintageBookshelf.Domain.Models;
using VintageBookshelf.Domain.Notifications;
using VintageBookshelf.Domain.Services;

namespace VintageBookshelf.Api.Controllers
{
    [Route("api/bookshelves")]
    public class BookshelvesController : MainController
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
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BookshelfDto>>> GetAll()
        {
            var bookshelves = _mapper.Map<IEnumerable<AuthorDto>>(await _bookshelfRepository.GetAll());
            return CustomResponse(bookshelves);
        }
        
        [HttpGet("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BookshelfDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookshelfDto>> GetById(long id)
        {
            var bookshelf = _mapper.Map<BookshelfDto>(await _bookshelfRepository.GetBookshelfWithProducts(id));
            if (bookshelf is null)
            {
                return NotFound();
            }
            return Ok(bookshelf);
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BookshelfDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BookshelfDto>> Add([FromBody] BookshelfDto bookshelfDto)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }
            await _bookshelfService.Add(_mapper.Map<Bookshelf>(bookshelfDto));
            return CustomResponse(bookshelfDto);
        }
        
        [HttpPut("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BookshelfDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BookshelfDto>> Update(long id, [FromBody] BookshelfDto bookshelfDto)
        {
            if (id != bookshelfDto.Id)
            {
                NotifyError("The given id in the body paylod doesn't match with the given id in the path!");
                return CustomResponse(bookshelfDto);
            }
            
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            await _bookshelfService.Update(_mapper.Map<Bookshelf>(bookshelfDto));
            return CustomResponse(bookshelfDto);
        }
        
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookshelfDto>> Remove(long id)
        {
            var author = _mapper.Map<BookshelfDto>(await _bookshelfRepository.GetById(id));
            if (author is null)
            {
                return NotFound();
            }

            await _bookshelfService.Remove(id);
            return CustomResponse();
        }
        
    }
}