namespace CalibrationApp.Models
{
    public class Answer
    {
        public int CalibrationId { get; set; }
        public int UserId { get; set; }
        public int QuestionId { get; set; }
        public int OptionId { get; set; }
        public string Comment { get; set; }
    }
}
