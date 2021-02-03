using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using VintageBookshelf.Domain.Models;

namespace VintageBookshelf.UI.ViewModels
{
    public class AuthorViewModel
    {
        [Key]
        public long Id { get; set; }
        
        [Required(ErrorMessage = "Field {0} is required")]
        [StringLength(200, ErrorMessage = "Field {0} needs to be {2} and {1} characters long", MinimumLength = 0)]
        public string Name { get; set; }
        
        // [DisplayName("Birth Date")]
        // [Required(ErrorMessage = "Field {0} is required")]
        // [StringLength(200, ErrorMessage = "Field {0} needs to be {2} and {1} characters long", MinimumLength = 0)]
        public DateTimeOffset BirthDate { get; set; }
        
        [Required(ErrorMessage = "Field {0} is required")]
        [StringLength(1000, ErrorMessage = "Field {0} needs to be {2} and {1} characters long", MinimumLength = 0)]
        public string Biography { get; set; }
        
        //public IEnumerable<BookViewModel> Books { get; set; }
    }
}