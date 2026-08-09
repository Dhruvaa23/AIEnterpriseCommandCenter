using AIEnterpriseCommandCenter.Application.DTOs.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIEnterpriseCommandCenter.Application.Interfaces
{
    public interface INotificationRepository
    {
        Task<List<NotificationDto>> GetAllAsync();

        Task<int> GetUnreadCountAsync();

        Task<bool> MarkAsReadAsync(int id);
        Task CreateAsync(string title, string message, string type);

        Task<List<NotificationDto>> GetLatestAsync(int count);


        Task<bool> DeleteAsync(int id);

        Task MarkAllAsReadAsync();

    }
}
