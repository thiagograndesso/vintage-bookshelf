using System;
using System.Collections.Generic;

namespace VintageBookshelf.Domain.Models
{
    public class Author : Entity
    {
        public string Name { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public string Biography { get; set; }

        public IEnumerable<Book> Books { get; set; }
    }
}