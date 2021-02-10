using System.ComponentModel.DataAnnotations;
using FluentValidation;
using VintageBookshelf.Domain.Models;
using VintageBookshelf.Domain.Notifications;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace VintageBookshelf.Domain.Services
{
    public abstract class BaseService
    {
        private readonly INotifier _notifier;

        protected BaseService(INotifier notifier)
        {
            _notifier = notifier;
        }
        
        protected void Notify(ValidationResult result)
        {
            foreach (var error in result.Errors)
            {
                Notify(error.ErrorMessage);
            }
        }
        
        protected void Notify(string message)
        {
            _notifier.Handle(new Notification(message));
        }

        protected bool Validate<TValidator, TEntity>(TValidator validator, TEntity entity)
            where TValidator : AbstractValidator<TEntity> where TEntity : Entity
        {
            var result = validator.Validate(entity);
            if (result.IsValid)
            {
                return true;
            }
            
            Notify(result);
            return false;
        }
    }
}