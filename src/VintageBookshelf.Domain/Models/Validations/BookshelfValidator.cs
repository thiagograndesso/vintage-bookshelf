using FluentValidation;

namespace VintageBookshelf.Domain.Models.Validations
{
    public class BookshelfValidator : AbstractValidator<Bookshelf>
    {
        public BookshelfValidator()
        {
            RuleFor(b => b.Name)
                .Length(1, 200)
                .NotEmpty();
            
            RuleFor(b => b.Address)
                .Length(1, 200)
                .NotEmpty();
            
            RuleFor(b => b.City)
                .Length(1, 200)
                .NotEmpty();
        }
    }
}