using System.ComponentModel.DataAnnotations;

namespace VintageBookshelf.Api.Dtos
{
    public class BookDto
    {
        [Key]
        public long Id { get; set; }
        
        [Required(ErrorMessage = "Field {0} is required")]
        [StringLength(200, ErrorMessage = "Field {0} needs to be {2} and {1} characters long", MinimumLength = 0)]
        public string Title { get; set; }
        
        [Required(ErrorMessage = "Field {0} is required")]
        [StringLength(200, ErrorMessage = "Field {0} needs to be {2} and {1} characters long", MinimumLength = 0)]
        public string Publisher { get; set; }
        
        [Required(ErrorMessage = "Field {0} is required")]
        public int ReleaseYear { get; set; }
        
        [Required(ErrorMessage = "Field {0} is required")]
        [StringLength(1000, ErrorMessage = "Field {0} needs to be {2} and {1} characters long", MinimumLength = 0)]
        public string Summary { get; set; }

        public string UploadImage { get; set; }

        public string Image { get; set; }

        public string AuthorName { get; set; }

        public string BookshelfName { get; set; }
    }
}