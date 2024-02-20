using BlazorPoultryDashboard.Application.Notifications.Events;
using BlazorPoultryDashboard.Models;

namespace BlazorPoultryDashboard.Domain.Interfaces
{
    public interface IThresholdLimitNotifier
    {
        event EventHandler<BirdEventArgs> MaxThresholdAlarm;
        event EventHandler<BirdEventArgs> MinThresholdAlarm;

        void MaxThresholdReached(Bird bird);
        void MinThresholdReached(Bird bird);
    }
}