using AIEnterpriseCommandCenter.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AIEnterpriseCommandCenter.Web.ViewComponents
{
    public class NotificationViewComponent : ViewComponent
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationViewComponent(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var count = await _notificationRepository.GetUnreadCountAsync();

            return View(count);
        }
    }
}