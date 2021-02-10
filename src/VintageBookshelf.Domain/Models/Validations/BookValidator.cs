using FluentValidation;

namespace VintageBookshelf.Domain.Models.Validations
{
    public class BookValidator : AbstractValidator<Book>
    {
        public BookValidator()
        {
            RuleFor(b => b.Title)
                .NotEmpty()
                .Length(1, 100);
            
            RuleFor(b => b.Summary)
                .NotEmpty()
                .Length(1, 1000);
            
            RuleFor(b => b.Publisher)
                .NotEmpty()
                .Length(1, 100);

            RuleFor(b => b.ReleaseYear)
                .GreaterThan(0)
                .NotEmpty();
        }
    }
}