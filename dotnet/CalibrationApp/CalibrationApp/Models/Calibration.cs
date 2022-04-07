namespace CalibrationApp.Models
{
    public class Calibration
    {
        public int Id { get; set; }
        public string RepFirstName { get; set; }
        public string RepLastName { get; set; }
        public decimal GroupScoreEarned { get; set; }
        public decimal GroupScorePossible { get; set; }
        public string ContactChannel { get; set; }
        public DateTime CalibrationDate { get; set; }
        public string ContactId { get; set; }
        public Boolean IsOpen { get; set; }
        public decimal IndivPointsEarned { get; set; }
        public decimal IndivPointsPossible { get; set; }
    }
}
