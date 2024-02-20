namespace BlazorPoultryDashboard.Domain.Interfaces
{
    public interface IReportEventNotifier
    {
        event EventHandler ReportSaved;
        void ReportUpdated();
    }
}
