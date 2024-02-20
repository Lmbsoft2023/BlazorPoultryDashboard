using BlazorPoultryDashboard.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorPoultryDashboard.Application.Database.SQLCe
{
    public class ReportDataReceiver
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ReportDataReceiver(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<IEnumerable<Report>> GetReportsAsync()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<BirdDbContext>();
                // Fetch reports.
                var reports = await context.Reports.ToListAsync();
                //Return Enumerable.Empty instead of null.
                return reports.Count() >= 1 ? reports : Enumerable.Empty<Report>()!;
            }
        }

        public async Task<IEnumerable<Report>> GetReportsAsync(TimeSpan timeWindow)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<BirdDbContext>();

                // Calculate the start time for the time window
                var startTime = DateTime.Now - timeWindow;

                // Fetch reports within the specified time window
                var reports = await context.Reports
                    .Where(r => r.CreatedAt >= startTime)
                    .ToListAsync();

                // Return Enumerable.Empty instead of null
                return reports.Any() ? reports : Enumerable.Empty<Report>();
            }
        }
    }
}
