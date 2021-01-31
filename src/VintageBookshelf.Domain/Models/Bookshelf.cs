using System.Collections.Generic;

namespace VintageBookshelf.Domain.Models
{
    public class Bookshelf : Entity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }

        public IEnumerable<Book> Books { get; set; }
    }
}