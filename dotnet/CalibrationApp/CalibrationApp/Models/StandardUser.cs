namespace CalibrationApp.Models
{
    /// <summary>
    /// Model of user data to return upon successful login
    /// </summary>
    public class StandardUser
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public int TeamId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public int? CalibrationPosition { get; set; }
        public decimal? PointsEarned { get; set; }
        public decimal? PointsPossible { get; set; }
        public List<Answer>? Answers { get; set; }
    }
}
