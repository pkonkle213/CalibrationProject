namespace CalibrationApp.Models
{
    public class Option
    {
        public int Id { get; set; }
        public int OrderPosition { get; set; }
        public string OptionValue { get; set; }
        public decimal PointsEarned { get; set; }
        public bool IsCategory { get; set; }
        public int FormId { get; set; }
        public bool? HasValue { get; set; }
        public bool? IsEnabled { get; set; }
    }
}