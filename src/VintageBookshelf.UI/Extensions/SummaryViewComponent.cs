using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VintageBookshelf.Domain.Notifications;

namespace VintageBookshelf.UI.Extensions
{
    public class SummaryViewComponent : ViewComponent
    {
        private readonly INotifier _notifier;

        public SummaryViewComponent(INotifier notifier)
        {
            _notifier = notifier;
        }
        
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var notifications = _notifier.GetNotifications();
            notifications.ForEach(n => ViewData.ModelState.AddModelError(string.Empty, n.Message));
            return View();
        }
    }
}