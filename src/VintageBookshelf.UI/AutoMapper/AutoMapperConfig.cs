using AutoMapper;
using VintageBookshelf.Domain.Models;
using VintageBookshelf.UI.ViewModels;

namespace VintageBookshelf.UI.AutoMapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Book, BookViewModel>().ReverseMap();
            CreateMap<Bookshelf, BookshelfViewModel>().ReverseMap();
            CreateMap<Author, AuthorViewModel>().ReverseMap();
        }
    }
}