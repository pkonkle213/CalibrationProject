namespace CalibrationApp.Models
{
    public class Calibration
    {
        public int Id { get; set; }
        public string RepFirstName { get; set; }
        public string RepLastName { get; set; }
        public int FormId { get; set; }
        public decimal GroupScoreEarned { get; set; }
        public decimal GroupScorePossible { get; set; }
        public int ContactChannelId { get; set; }
        public DateTime CalibrationDate { get; set; }
        public string ContactId { get; set; }
        public bool IsOpen { get; set; }
        public int LeaderUserId { get; set; }
    }
}
