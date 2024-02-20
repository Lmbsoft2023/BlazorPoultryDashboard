using BlazorPoultryDashboard.Models;

namespace BlazorPoultryDashboard.Application.Notifications.Events
{
    public class BirdEventArgs : EventArgs
    {
        public Bird Bird { get; }

        public BirdEventArgs(Bird bird)
        {
            Bird = bird;
        }
    }
}
