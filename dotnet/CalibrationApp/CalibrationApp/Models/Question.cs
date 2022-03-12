namespace CalibrationApp.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public bool IsCategory { get; set; }
        public int PointsPossible { get; set; }
        public List<Option> Options { get; set; }
    }
}
