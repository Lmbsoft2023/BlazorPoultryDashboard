using BlazorPoultryDashboard.Domain.Interfaces;

namespace BlazorPoultryDashboard.Application.Notifications
{
    public class ReportEventNotifier : IReportEventNotifier
    {
        public event EventHandler ReportSaved;

        public void ReportUpdated()
        {
            ReportSaved?.Invoke(this, EventArgs.Empty);
        }
    }
}
