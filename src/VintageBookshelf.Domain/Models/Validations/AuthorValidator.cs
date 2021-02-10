using FluentValidation;

namespace VintageBookshelf.Domain.Models.Validations
{
    public class AuthorValidator : AbstractValidator<Author>
    {
        public AuthorValidator()
        {
            RuleFor(a => a.Name)
                .NotEmpty()
                .Length(1, 100);

            RuleFor(a => a.Biography)
                .NotEmpty()
                .Length(1, 1000);
            
            RuleFor(a => a.BirthDate)
                .NotNull();
        }
    }
}