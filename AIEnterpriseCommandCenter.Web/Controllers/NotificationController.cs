using AIEnterpriseCommandCenter.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIEnterpriseCommandCenter.Web.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationController(
            INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        //====================================
        // Notification List
        //====================================

        public async Task<IActionResult> All()
        {
            var notifications = await _notificationRepository.GetAllAsync();

            return View(notifications);
        }

        //====================================
        // Mark Read
        //====================================

        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _notificationRepository.MarkAsReadAsync(id);

            return Ok();
        }


        //====================================
        // Mark All Read
        //====================================

        [HttpPost]
        public async Task<IActionResult> MarkAllRead()
        {
            await _notificationRepository.MarkAllAsReadAsync();

            TempData["Success"] = "All notifications marked as read.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _notificationRepository.DeleteAsync(id);

            return Ok();
        }
    }
}