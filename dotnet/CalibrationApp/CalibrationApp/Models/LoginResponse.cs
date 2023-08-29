namespace CalibrationApp.Models
{
    /// <summary>
    /// Model to return upon successful login (user data + token)
    /// </summary>
    public class LoginResponse
    {
        public StandardUser User { get; set; }
        public string Token { get; set; }
    }
}
