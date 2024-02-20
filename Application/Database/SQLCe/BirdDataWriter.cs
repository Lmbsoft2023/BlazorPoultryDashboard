using BlazorPoultryDashboard.Domain.Interfaces;
using BlazorPoultryDashboard.Models;

namespace BlazorPoultryDashboard.Application.Database.SQLCe
{
    public class BirdDataWriter
    {
        private readonly BirdDbContext _context;
        private readonly IThresholdLimitNotifier _thresholdLimitNotifier;
        private readonly IConfiguration _configuration;
        public BirdDataWriter(BirdDbContext context, IThresholdLimitNotifier thresholdLimitNotifier, IConfiguration configuration)
        {
            _context = context;
            _thresholdLimitNotifier = thresholdLimitNotifier;
            _configuration = configuration;
        }

        public async Task<bool> WriteBirdDataAsync(List<WeightData> weights, List<GradeData> grades, List<DropOffData> dropoffs)
        {
            if (weights.Count == 1 && grades.Count == 1 && dropoffs.Count == 1)
            {
                WeightData? weightData = weights.FirstOrDefault()!;
                GradeData? gradeData = grades.FirstOrDefault()!;
                DropOffData? dropOffData = dropoffs.FirstOrDefault()!;

                if (dropOffData == null)
                    return false;

                Guid traceId = Guid.NewGuid();

                var bird = new Bird
                {
                    TracingId = traceId,
                    WeightShackleId = weightData.ShackleId!,
                    GradeShackleId = gradeData.ShackleId!,
                    DropOffShackleId = dropOffData.ShackleId!,
                    WeightDate = weightData.Date,
                    GradeDate = gradeData.Date,
                    DropOffDate = gradeData.Date,
                    Weight = weightData.WeightInGram,
                    Grade = gradeData.Grade,
                    DropOff = dropOffData.DropOff
                };
                decimal maxThreshold = Convert.ToDecimal(GetSessionStorageValue("max"));
                decimal minThreshold = Convert.ToDecimal(GetSessionStorageValue("min"));
                if ((int)weightData.WeightInGram >= maxThreshold)
                {
                    _thresholdLimitNotifier.MaxThresholdReached(bird);
                }
                else if ((int)weightData.WeightInGram <= minThreshold)
                {
                    _thresholdLimitNotifier.MinThresholdReached(bird);

                }

                await _context.Birds.AddAsync(bird);
                int saveResult = await _context.SaveChangesAsync();
                return saveResult > 0;
            }
            else if (weights.Count == 0 && grades.Count == 0 && dropoffs.Count == 0)
            {
                return false;
            }

            return false;
        }

        // Method to get a value from sessionStorage
        private string GetSessionStorageValue(string key)
        {
            return  _configuration[$"WeightThreshold:{key}"];
        }
    }
}
