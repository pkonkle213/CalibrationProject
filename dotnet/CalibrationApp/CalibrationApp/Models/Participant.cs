namespace CalibrationApp.Models
{
    public class Participant
    {
        public int UserId { get; set; }
        public string Role { get; set; }
        public string Team { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
    }
}
