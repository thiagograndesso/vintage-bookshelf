using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VintageBookshelf.Api.Controllers;
using VintageBookshelf.Api.Dtos;
using VintageBookshelf.Domain.Interfaces;
using VintageBookshelf.Domain.Models;
using VintageBookshelf.Domain.Notifications;
using VintageBookshelf.Domain.Services;

namespace VintageBookshelf.Api.V1.Controllers
{
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/bookshelves")]
    public class BookshelvesController : MainController
    {
        private readonly IBookshelfRepository _bookshelfRepository;
        private readonly IBookshelfService _bookshelfService;
        private readonly IMapper _mapper;

        public BookshelvesController(IBookshelfRepository bookshelfRepository,
                                     IBookshelfService bookshelfService,
                                     IMapper mapper,
                                     INotifier notifier,
                                     IUser user) : base(notifier, user)
        {
            _bookshelfRepository = bookshelfRepository;
            _bookshelfService = bookshelfService;
            _mapper = mapper;
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BookshelfDto>>> GetAll()
        {
            var bookshelves = _mapper.Map<IEnumerable<BookshelfDto>>(await _bookshelfRepository.GetAllWithProducts());
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
            var bookshelf = await _bookshelfRepository.GetById(id);
            if (bookshelf is null)
            {
                return NotFound();
            }

            await _bookshelfService.Remove(id);
            return CustomResponse();
        }
        
    }
}