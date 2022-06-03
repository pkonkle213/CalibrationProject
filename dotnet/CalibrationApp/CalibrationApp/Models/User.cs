namespace CalibrationApp.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public string Role { get; set; }
        public string Team { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public List<Answer> Answers { get; set; }
        public decimal PointsEarned { get; set; }
        public decimal PointsPossible { get; set; }
    }
}
