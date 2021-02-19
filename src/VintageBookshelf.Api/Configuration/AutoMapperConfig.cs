using AutoMapper;
using VintageBookshelf.Api.Dtos;
using VintageBookshelf.Domain.Models;

namespace VintageBookshelf.Api.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Author, AuthorDto>().ReverseMap();
            CreateMap<Book, BookDto>().ReverseMap();
            CreateMap<Bookshelf, BookshelfDto>().ReverseMap();
        }
    }
}