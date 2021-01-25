namespace VintageBookshelf.Domain.Models
{
    public class Book : Entity
    {
        public string Title { get; set; }
        public string Publisher { get; set; }
        public int ReleaseYear { get; set; }
        public string Summary { get; set; }

        public Author Author { get; set; }
    }
}