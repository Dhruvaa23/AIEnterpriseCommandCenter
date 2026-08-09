using AIEnterpriseCommandCenter.Application.DTOs.Notification;
using AIEnterpriseCommandCenter.Application.Interfaces;
using AIEnterpriseCommandCenter.Domain.Entities;
using AIEnterpriseCommandCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AIEnterpriseCommandCenter.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        //===========================================
        // Get All Notifications
        //===========================================

        public async Task<List<NotificationDto>> GetAllAsync()
        {
            return await _context.Notifications
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => new NotificationDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Message = x.Message,
                    Type = x.Type,
                    IsRead = x.IsRead,
                    CreatedOn = x.CreatedOn
                })
                .ToListAsync();
        }

        //===========================================
        // Get Unread Count
        //===========================================

        public async Task<int> GetUnreadCountAsync()
        {
            return await _context.Notifications
                .CountAsync(x => !x.IsRead);
        }

        //===========================================
        // Mark As Read
        //===========================================

        public async Task<bool> MarkAsReadAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);

            if (notification == null)
                return false;

            notification.IsRead = true;

            await _context.SaveChangesAsync();

            return true;
        }

        //===========================================
        // Create Notification
        //===========================================

        public async Task CreateAsync(
            string title,
            string message,
            string type)
        {
            var notification = new Notification
            {
                Title = title,
                Message = message,
                Type = type,
                IsRead = false,
                CreatedOn = DateTime.Now
            };

            _context.Notifications.Add(notification);

            await _context.SaveChangesAsync();
        }

        public async Task<List<NotificationDto>> GetLatestAsync(int count)
        {
            return await _context.Notifications
                .OrderByDescending(x => x.CreatedOn)
                .Take(count)
                .Select(x => new NotificationDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Message = x.Message,
                    IsRead = x.IsRead,
                    CreatedOn = x.CreatedOn
                })
                .ToListAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);

            if (notification == null)
                return false;

            _context.Notifications.Remove(notification);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task MarkAllAsReadAsync()
        {
            var notifications = await _context.Notifications
                .Where(x => !x.IsRead)
                .ToListAsync();

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }

            await _context.SaveChangesAsync();
        }
    }

}