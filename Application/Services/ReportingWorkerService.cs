namespace BlazorPoultryDashboard.Application.Services
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using BlazorPoultryDashboard.Application.Database.SQLCe;
    using BlazorPoultryDashboard.Domain.Interfaces;
    using BlazorPoultryDashboard.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class ReportingWorkerService : BackgroundService
    {
        private readonly ILogger<ReportingWorkerService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public readonly IReportEventNotifier _reportEventNotifier;

        public ReportingWorkerService(ILogger<ReportingWorkerService> logger, IServiceScopeFactory serviceScopeFactory, IReportEventNotifier reportEventNotifier)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _reportEventNotifier = reportEventNotifier;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Reporting worker is running at: {time}", DateTimeOffset.Now);

                GenerateReport();

                await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken); // Adjust interval as needed
            }
        }

        private void GenerateReport()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BirdDbContext>();
                var birds = dbContext.Birds.ToList(); // Assuming Birds is your DbSet<Bird> in BirdDbContext

                if(birds == null || birds.Count == 0)
                    return;

                double averageWeight = birds.Average(b => b.Weight);
                double meanGrade = birds.Average(b => b.Grade);
                // Calculate other statistics as needed

                // Calculate average rate of bird weight
                TimeSpan elapsedTime = DateTime.Now - birds.Min(b => b.WeightDate);
                double totalWeightChange = birds.Max(b => b.Weight) - birds.Min(b => b.Weight);
                double averageWeightRate = totalWeightChange / elapsedTime.TotalMinutes;

                // Calculate different KPIs for bird weight
                double minWeight = birds.Min(b => b.Weight);
                double maxWeight = birds.Max(b => b.Weight);
                double medianWeight = birds.OrderBy(b => b.Weight).Skip(birds.Count / 2).First().Weight;

                var report = new Report() {
                    AverageWeight = averageWeight,
                     MeanGrade = meanGrade,
                      MedianWeight = medianWeight,
                       MaxWeight = maxWeight,
                        AverageWeightRate = averageWeightRate,
                         MinWeight = minWeight,
                          TotalWeightChange = totalWeightChange,
                           CreatedAt = DateTime.Now  
                };

                 dbContext.Reports.Add(report);
                int saveResult = dbContext.SaveChanges();

                if (saveResult > 0)
                    _reportEventNotifier.ReportUpdated();
            }
        }
    }

}
