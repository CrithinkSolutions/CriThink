﻿using System.Threading.Tasks;
using CriThink.Server.Application.Administration.ViewModels;

namespace CriThink.Server.Application.Queries
{
    public interface INotificationQueries
    {
        /// <summary>
        /// Get the list of all notifications in pending.
        /// With pagination support
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns></returns>
        Task<NotificationRequestGetAllViewModel> GetAllNotificationsAsync(int pageSize, int pageIndex);
    }
}
