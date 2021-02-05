namespace VintageBookshelf.Domain.Models
{
    public class Book : Entity
    {
        public string Title { get; set; }
        public string Publisher { get; set; }
        public int ReleaseYear { get; set; }
        public string Summary { get; set; }
        public long AuthorId { get; set; }
        public long BookshelfId { get; set; }
        public string Image { get; set; }

        public virtual Author Author { get; set; }
        public virtual Bookshelf Bookshelf { get; set; }
    }
}