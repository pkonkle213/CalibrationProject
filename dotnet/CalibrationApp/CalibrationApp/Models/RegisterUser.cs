namespace CalibrationApp.Models
{
    /// <summary>
    /// Model to accept registration parameters
    /// </summary>
    public class RegisterUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Role { get; set; }
    }
}
