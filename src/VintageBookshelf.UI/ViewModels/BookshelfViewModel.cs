using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VintageBookshelf.Domain.Models;

namespace VintageBookshelf.UI.ViewModels
{
    public class BookshelfViewModel
    {
        [Key]
        public long Id { get; set; }
        
        [Required(ErrorMessage = "Field {0} is required")]
        [StringLength(200, ErrorMessage = "Field {0} needs to be {2} and {1} characters long", MinimumLength = 0)]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Field {0} is required")]
        [StringLength(200, ErrorMessage = "Field {0} needs to be {2} and {1} characters long", MinimumLength = 0)]
        public string Address { get; set; }
        
        [Required(ErrorMessage = "Field {0} is required")]
        [StringLength(200, ErrorMessage = "Field {0} needs to be {2} and {1} characters long", MinimumLength = 0)]
        public string City { get; set; }

        public IEnumerable<Book> Books { get; set; }
    }
}