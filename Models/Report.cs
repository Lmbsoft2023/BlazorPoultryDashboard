namespace BlazorPoultryDashboard.Models
{
    public class Report
    {
        public int Id { get; set; }
        public double AverageWeight { get; set; }
        public double MeanGrade { get; set; }
        public double TotalWeightChange { get; set; }
        public double AverageWeightRate { get; set; }
        public double MinWeight { get; set; }
        public double MaxWeight { get; set; }
        public double MedianWeight { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
