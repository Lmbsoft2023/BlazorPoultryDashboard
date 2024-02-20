using BlazorPoultryDashboard.Domain.Interfaces;

namespace BlazorPoultryDashboard.Application.Notifications
{
    public class DbEventNotifier : IDbEventNotifier
    {
       
        public event EventHandler DataSaved;

        public void ChangeOccurred()
        {
            DataSaved?.Invoke(this, EventArgs.Empty);
        }
    }

}
