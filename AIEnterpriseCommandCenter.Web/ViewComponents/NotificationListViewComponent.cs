using AIEnterpriseCommandCenter.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AIEnterpriseCommandCenter.Web.ViewComponents
{
    public class NotificationListViewComponent : ViewComponent
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationListViewComponent(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var notifications = await _notificationRepository.GetLatestAsync(3);

            return View(notifications);
        }
    }
}