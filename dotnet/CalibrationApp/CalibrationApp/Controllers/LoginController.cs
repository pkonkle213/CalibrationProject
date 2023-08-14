using Microsoft.AspNetCore.Mvc;
using CalibrationApp.DAO;
using CalibrationApp.Models;
using CalibrationApp.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CalibrationApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ITokenGenerator tokenGenerator;
        private readonly IPasswordHasher passwordHasher;
        private readonly IUserDAO userDAO;

        public LoginController(ITokenGenerator _tokenGenerator, IPasswordHasher _passwordHasher, IUserDAO _userDAO)
        {
            tokenGenerator = _tokenGenerator;
            passwordHasher = _passwordHasher;
            userDAO = _userDAO;
        }
        
        [HttpGet("Teams")]
        public ActionResult<List<Team>> GetAllTeams()
        {
            return Ok(userDAO.GetTeams());
        }

        [HttpGet("Roles")]
        public ActionResult<List<Role>> GetAllRoles()
        {
            return Ok(userDAO.GetRoles());
        }

        [HttpGet("Users")]
        public ActionResult<List<User>> GetAllUsers()
        {
            return Ok(userDAO.GetAllUsers());
        }

        /// <summary>
        /// A test endpoint to ensure that the server is running.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("Ready")]
        public string Ready()
        {
            return "Server Ready";
        }

        /// <summary>
        /// A test endpoint requiring authorization to ensure that authorization is working.
        /// </summary>
        [Authorize]
        [HttpGet("Confirm")]
        public string Confirm()
        {
            return "Token confirmed";
        }

        /// <summary>
        /// A test endpoint requiring authorization and the role of 'admin' to ensure that authorization is working.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("confirmadmin")]
        public string ConfirmAdmin()
        {
            return "Token confirmed for admin";
        }

        /// <summary>
        /// Authenticates the user and returns a result including their authenticated token
        /// </summary>
        /// <param name="userParam">The login request</param>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Authenticate(LoginUser userParam)
        {
            // Default to bad username/password message
            IActionResult result = Unauthorized(new { message = "Username or password is incorrect" });

            // Get the user by username
            User user = userDAO.GetUser(userParam.Username);

            // If we found a user and the password hash matches
            if (user != null && passwordHasher.VerifyHashMatch(user.PasswordHash, userParam.Password, user.Salt))
            {
                // Create an authentication token
                string token = tokenGenerator.GenerateToken(user.UserId, user.Username, user.Role);

                // Create a ReturnUser object to return to the client
                LoginResponse retUser = new LoginResponse()
                {
                    User = new ReturnUser()
                    {
                        UserId = user.UserId,
                        Username = user.Username,
                        Role = user.Role,
                        Team = user.Team,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                    },
                    Token = token
                };

                // Switch to 200 OK
                result = Ok(retUser);
            }

            return result;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="userParam">The new user to create</param>
        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult Register(RegisterUser userParam)
        {
            IActionResult result;

            User existingUser = userDAO.GetUser(userParam.Username);
            if (existingUser != null)
            {
                return Conflict(new { message = "Username already taken. Please choose a different username." });
            }

            User user = userDAO.AddUser(userParam.Username, userParam.Password, userParam.Role, userParam.isActive, userParam.Team, userParam.FirstName, userParam.LastName);
            if (user != null)
            {
                result = Created(user.Username, null);
            }
            else
            {
                result = BadRequest(new { message = "An error occurred and user was not created." });
            }

            return result;
        }

        [HttpPut]
        public IActionResult ChangeActive(int userId)
        {
            userDAO.SwitchActive(userId);
            return Ok("Switched!");
        }
    }
}
