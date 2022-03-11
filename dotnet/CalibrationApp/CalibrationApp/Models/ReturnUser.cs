namespace CalibrationApp.Models
{
    /// <summary>
    /// Model of user data to return upon successful login
    /// </summary>
    public class ReturnUser
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string Team { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
    }
}
