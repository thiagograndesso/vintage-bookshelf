using Microsoft.AspNetCore.Mvc;
using VintageBookshelf.Domain.Notifications;

namespace VintageBookshelf.UI.Controllers
{
    public class BaseController : Controller
    {
        private readonly INotifier _notifier;

        public BaseController(INotifier notifier)
        {
            _notifier = notifier;
        }

        protected bool IsOperationValid()
        {
            return !_notifier.HasNotification();
        }
    }
}