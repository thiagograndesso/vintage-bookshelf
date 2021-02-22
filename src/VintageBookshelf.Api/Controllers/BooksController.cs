using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VintageBookshelf.Api.Dtos;
using VintageBookshelf.Domain.Interfaces;
using VintageBookshelf.Domain.Models;
using VintageBookshelf.Domain.Notifications;
using VintageBookshelf.Domain.Services;
using static System.IO.File;

namespace VintageBookshelf.Api.Controllers
{
    [Route("api/books")]
    public class BooksController : MainController
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;

        public BooksController(IBookRepository bookRepository,
                               IBookService bookService,
                               IMapper mapper,
                               INotifier notifier) : base(notifier)
        {
            _bookRepository = bookRepository;
            _bookService = bookService;
            _mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<BookDto>>> Add([FromBody] BookDto bookDto)
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
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetAll()
        {
            var books = _mapper.Map<IEnumerable<BookDto>>(await _bookRepository.GetAllWithAuthorAndBookshelf());
            return CustomResponse(books);
        }
        
        [HttpGet("{id:long}")]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetById(long id)
        {
            var book = _mapper.Map<IEnumerable<BookDto>>(await _bookRepository.GetById(id));
            if (book == null)
            {
                return NotFound();
            }
            return CustomResponse(book);
        }
        
        [HttpDelete("{id:long}")]
        public async Task<ActionResult<IEnumerable<BookDto>>> Remove(long id)
        {
            var book = _mapper.Map<IEnumerable<BookDto>>(await _bookRepository.GetById(id));
            if (book == null)
            {
                return NotFound();
            }

            await _bookService.Remove(id);
            return CustomResponse(book);
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
            return true;

        }
    }
}