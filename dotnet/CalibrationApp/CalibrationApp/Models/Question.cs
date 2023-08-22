namespace CalibrationApp.Models
{
    public class Question
    {
        public int Id { get; set; }
        public int FormId { get; set; }
        public string QuestionText { get; set; }
        public int PointsPossible { get; set; }
        public bool IsCategory => PointsPossible > 0;
        public int FormPosition { get; set; }
        //public List<Option> Options { get; set; }
    }
}
