namespace BlazorPoultryDashboard.Models
{
    public class Bird
    {
        public int Id { get; set; }
        public Guid? TracingId { get; set; }
        public int WeightShackleId { get; set; }
        public int GradeShackleId { get; set; }
        public int DropOffShackleId { get; set; }
        public DateTime WeightDate { get; set; }
        public DateTime GradeDate { get; set; }
        public DateTime DropOffDate { get; set; }
        public double Weight { get; set; }
        public int Grade { get; set; }
        public int DropOff { get; set; }
    }
}
