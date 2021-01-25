namespace VintageBookshelf.Domain.Models
{
    public abstract class Entity
    {
        protected Entity()
        {
            
        }

        public long Id { get; set; }
    }
}