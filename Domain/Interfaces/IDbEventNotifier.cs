namespace BlazorPoultryDashboard.Domain.Interfaces
{
    public interface IDbEventNotifier
    {
        event EventHandler DataSaved;
        void ChangeOccurred();
    }
}