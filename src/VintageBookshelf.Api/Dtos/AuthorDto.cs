using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VintageBookshelf.Api.Dtos
{
    public class AuthorDto
    {
        [Key]
        public long Id { get; set; }
        
        [Required(ErrorMessage = "Field {0} is required")]
        [StringLength(200, ErrorMessage = "Field {0} needs to be {2} and {1} characters long", MinimumLength = 0)]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Field {0} is required")]
        public DateTimeOffset BirthDate { get; set; }
        
        [Required(ErrorMessage = "Field {0} is required")]
        [StringLength(1000, ErrorMessage = "Field {0} needs to be {2} and {1} characters long", MinimumLength = 0)]
        public string Biography { get; set; }
        
        public IEnumerable<BookDto> Books { get; set; }
    }
}