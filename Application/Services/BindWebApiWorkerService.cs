using BlazorPoultryDashboard.Application.Database.SQLCe;
using BlazorPoultryDashboard.Domain.Interfaces;
using BlazorPoultryDashboard.Models;

namespace BlazorPoultryDashboard.Application.Services
{
    public class BindWebApiWorkerService : BackgroundService
    {
        private readonly ILogger<BindWebApiWorkerService> _logger;
        private readonly HttpClient _httpClient;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly TimeSpan _interval = TimeSpan.FromSeconds(1);
        public readonly IDbEventNotifier _dbEventNotifier;


        public BindWebApiWorkerService(ILogger<BindWebApiWorkerService> logger, HttpClient httpClient, IServiceScopeFactory serviceScopeFactory, IDbEventNotifier dbEventNotifier)
        {
            _logger = logger;
            _httpClient = httpClient;
            _serviceScopeFactory = serviceScopeFactory;
            _dbEventNotifier = dbEventNotifier;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Background worker is running at: {time}", DateTimeOffset.Now);

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedBirdDataWriter = scope.ServiceProvider.GetRequiredService<BirdDataWriter>();

                    var weightData = await _httpClient.GetFromJsonAsync<List<WeightData>>("https://localhost:44333/ChickenWeightData");
                    var gradeData = await _httpClient.GetFromJsonAsync<List<GradeData>>("https://localhost:44333/ChickenGradeData");
                    var dropOffData = await _httpClient.GetFromJsonAsync<List<DropOffData>>("https://localhost:44333/ChickenDropOffData");

                    if (weightData != null && gradeData != null && dropOffData != null)
                    {
                        await scopedBirdDataWriter.WriteBirdDataAsync(weightData, gradeData, dropOffData);
                        _dbEventNotifier.ChangeOccurred();
                    }
                    else
                    {
                        _logger.LogError($"Failed to fetch chicken data!");
                    }
                }

                await Task.Delay(_interval, stoppingToken);
            }
        }
    }
}
