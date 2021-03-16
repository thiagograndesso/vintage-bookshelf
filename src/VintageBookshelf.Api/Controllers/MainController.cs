using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using VintageBookshelf.Domain.Interfaces;
using VintageBookshelf.Domain.Notifications;

namespace VintageBookshelf.Api.Controllers
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        private readonly INotifier _notifier;

        protected bool IsUserAuthenticated { get; }
        protected Guid UserId { get; }

        public MainController(INotifier notifier, IUser appUser)
        {
            _notifier = notifier;
            
            if (appUser.IsAuthenticated())
            {
                UserId = appUser.GetUserId();
                IsUserAuthenticated = true;
            }
        }
        
        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                NotifyInvalidModelState(modelState);
            }
            return CustomResponse();
        }

        protected ActionResult CustomResponse(object result = null)
        {
            if (IsOperationValid())
            {
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }

            return BadRequest(new {success = false, errors = _notifier.GetNotifications().Select(n => n.Message)});
        }

        protected bool IsOperationValid()
        {
            return !_notifier.HasNotification();
        }

        private void NotifyInvalidModelState(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(m => m.Errors);
            foreach (var error in errors)
            {
                var errorMessage = error.Exception is null ? error.ErrorMessage : error.Exception.Message;
                NotifyError(errorMessage);
            }
        }

        protected void NotifyError(string message)
        {
            _notifier.Handle(new Notification(message));
        }
        
        
    }
}