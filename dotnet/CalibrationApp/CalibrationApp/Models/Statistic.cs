namespace CalibrationApp.Models
{
    public class Statistic
    {
        public int ElementId { get; set; }
        public string Description { get; set; }
        public int Correct { get; set; }
        public int Possible { get; set; }
    }
}
