namespace CalibrationApp.Models
{
    public class Answer
    {
        public int CalibrationId { get; set; }
        public string OptionValue { get; set; }
        public int QuestionId { get; set; }
        public string Comment { get; set; }
        public decimal PointsEarned { get; set; }
    }
}
