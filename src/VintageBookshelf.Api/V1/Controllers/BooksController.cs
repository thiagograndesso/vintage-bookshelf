using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VintageBookshelf.Api.Controllers;
using VintageBookshelf.Api.Dtos;
using VintageBookshelf.Api.Extensions;
using VintageBookshelf.Domain.Interfaces;
using VintageBookshelf.Domain.Models;
using VintageBookshelf.Domain.Notifications;
using VintageBookshelf.Domain.Services;
using static System.IO.File;

namespace VintageBookshelf.Api.V1.Controllers
{
    [ApiVersion("1.0")]
    [Authorize]
    [Route("api/v{version:apiVersion}/books")]
    public class BooksController : MainController
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;

        public BooksController(IBookRepository bookRepository,
                               IBookService bookService,
                               IMapper mapper,
                               INotifier notifier,
                               IUser user) : base(notifier, user)
        {
            _bookRepository = bookRepository;
            _bookService = bookService;
            _mapper = mapper;
        }

        [ClaimsAuthorize("Book","Add")]
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<BookDto>> Add([FromBody] BookDto bookDto)
        {
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            if (!await UploadImage(bookDto))
            {
                return CustomResponse();
            }

            await _bookService.Add(_mapper.Map<Book>(bookDto));
            return CustomResponse(bookDto);
        }
        
        [ClaimsAuthorize("Book","Update")]
        [HttpPut("{id:long}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<BookDto>> Update(long id, [FromBody] BookDto bookDto)
        {
            if (id != bookDto.Id)
            {
                NotifyError("The given id doesn't match with object to update!");
                return NotFound();
            }
            
            var toUpdate = _mapper.Map<BookDto>(await _bookRepository.GetBookWithAuthorAndBookshelf(id));
            bookDto.Image = toUpdate.Image;
            
            if (!ModelState.IsValid)
            {
                return CustomResponse(ModelState);
            }

            if (bookDto.UploadImage is not null)
            {
                if (!await UploadImage(bookDto))
                {
                    return CustomResponse();
                }

                toUpdate.Image = bookDto.Image;
            }

            toUpdate.Publisher = bookDto.Publisher;
            toUpdate.ReleaseYear = bookDto.ReleaseYear;
            toUpdate.Title = bookDto.Title;

            await _bookService.Update(_mapper.Map<Book>(toUpdate));
            return CustomResponse(bookDto);
        }
        
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetAll()
        {
            var books = _mapper.Map<IEnumerable<BookDto>>(await _bookRepository.GetAllWithAuthorAndBookshelf());
            return CustomResponse(books);
        }
        
        [HttpGet("{id:long}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<BookDto>> GetById(long id)
        {
            var book = _mapper.Map<BookDto>(await _bookRepository.GetById(id));
            if (book == null)
            {
                return NotFound();
            }
            return CustomResponse(book);
        }
        
        [ClaimsAuthorize("Book","Remove")]
        [HttpDelete("{id:long}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Remove(long id)
        {
            var book = _mapper.Map<BookDto>(await _bookRepository.GetById(id));
            if (book == null)
            {
                return NotFound();
            }

            await _bookService.Remove(id);
            return CustomResponse();
        }

        private async Task<bool> UploadImage(BookDto bookDto)
        {
            if (string.IsNullOrEmpty(bookDto.UploadImage))
            {
                NotifyError("Please provide an image file!");
                return false;
            }

            var imageName = $"{Guid.NewGuid()}_{bookDto.Image}";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageName);

            if (Exists(filePath))
            {
                NotifyError("An image already exists!");
                return false;
            }

            var imageDataByteArray = Convert.FromBase64String(bookDto.UploadImage);
            await WriteAllBytesAsync(filePath, imageDataByteArray);

            bookDto.Image = imageName;
            return true;

        }
    }
}