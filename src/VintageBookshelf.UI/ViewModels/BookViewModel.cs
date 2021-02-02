using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VintageBookshelf.UI.ViewModels
{
    public class BookViewModel
    {
        [Key]
        public long Id { get; set; }
        
        [Required(ErrorMessage = "Field {0} is required")]
        [StringLength(200, ErrorMessage = "Field {0} needs to be {2} and {1} characters long", MinimumLength = 0)]
        public string Title { get; set; }
        
        [Required(ErrorMessage = "Field {0} is required")]
        [StringLength(200, ErrorMessage = "Field {0} needs to be {2} and {1} characters long", MinimumLength = 0)]
        public string Publisher { get; set; }
        
        [DisplayName("Release Year")]
        [Required(ErrorMessage = "Field {0} is required")]
        public int ReleaseYear { get; set; }
        
        [Required(ErrorMessage = "Field {0} is required")]
        [StringLength(1000, ErrorMessage = "Field {0} needs to be {2} and {1} characters long", MinimumLength = 0)]
        public string Summary { get; set; }

        //public IFormFile UploadImage { get; set; }
        
        [HiddenInput]
        public long AuthorId { get; set; }
        
        [HiddenInput]
        public long BookshelfId { get; set; }

        public virtual AuthorViewModel Author { get; set; }
        public virtual BookshelfViewModel Bookshelf { get; set; }
    }
}