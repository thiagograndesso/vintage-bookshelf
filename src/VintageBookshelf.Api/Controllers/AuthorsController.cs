using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VintageBookshelf.Api.Dtos;
using VintageBookshelf.Domain.Interfaces;
using VintageBookshelf.Domain.Models;
using VintageBookshelf.Domain.Notifications;
using VintageBookshelf.Domain.Services;

namespace VintageBookshelf.Api.Controllers
{
    [Route("api/authors")]
    public class AuthorsController : MainController
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IAuthorService _authorService;
        private readonly IMapper _mapper;
        
        public AuthorsController(IAuthorRepository authorRepository, 
                                 IAuthorService authorService, 
                                 IMapper mapper, 
                                 INotifier notifier,
                                 IUser user) : base(notifier, user)
        {
            _authorRepository = authorRepository;
            _authorService = authorService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAll()
        {
            var authors = _mapper.Map<IEnumerable<AuthorDto>>(await _authorRepository.GetAllWithBooks());
            return CustomResponse(authors);
        }
        
        [HttpGet("{id:long}")]
        [ProducesResponseType(200, Type = typeof(AuthorDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<AuthorDto>> GetById(long id)
        {
            var author = _mapper.Map<AuthorDto>(await _authorRepository.GetAuthorWithBooks(id));
            if (author is null)
            {
                return NotFound();
            }
            return Ok(author);
        }
        
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<AuthorDto>> Add([FromBody] AuthorDto authorDto)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }
            await _authorService.Add(_mapper.Map<Author>(authorDto));
            return CustomResponse(authorDto);
        }
        
        [HttpPut("{id:long}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<AuthorDto>> Update(long id, [FromBody] AuthorDto authorDto)
        {
            if (id != authorDto.Id)
            {
                NotifyError("The given id in the body paylod doesn't match with the given id in the path!");
                return CustomResponse(authorDto);
            }
            
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            await _authorService.Update(_mapper.Map<Author>(authorDto));
            return CustomResponse(authorDto);
        }
        
        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<AuthorDto>> Remove(long id)
        {
            var author = _mapper.Map<AuthorDto>(await _authorRepository.GetById(id));
            if (author is null)
            {
                return NotFound();
            }

            await _authorService.Remove(id);
            return CustomResponse();
        }
        
    }
}