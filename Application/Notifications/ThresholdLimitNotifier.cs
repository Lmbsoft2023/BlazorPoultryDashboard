using BlazorPoultryDashboard.Application.Notifications.Events;
using BlazorPoultryDashboard.Domain.Interfaces;
using BlazorPoultryDashboard.Models;

namespace BlazorPoultryDashboard.Application.Notifications
{
    public class ThresholdLimitNotifier : IThresholdLimitNotifier
    {
        public event EventHandler<BirdEventArgs> MaxThresholdAlarm;
        public event EventHandler<BirdEventArgs> MinThresholdAlarm;

        public void MaxThresholdReached(Bird bird)
        {
            MaxThresholdAlarm?.Invoke(this, new BirdEventArgs(bird));
        }

        public void MinThresholdReached(Bird bird)
        {
            MinThresholdAlarm?.Invoke(this, new BirdEventArgs(bird));
        }
    }
}

