using Microsoft.AspNetCore.Mvc;
using CalibrationApp.DAO;
using CalibrationApp.Models;
using CalibrationApp.Security;
using Microsoft.AspNetCore.Authorization;

namespace CalibrationApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize]
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

        [HttpGet("Users")]
        public ActionResult<List<SaltedUser>> GetAllUsers()
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
            SaltedUser user = userDAO.GetUser(userParam.Username);

            // If we found a user and the password hash matches
            if (user != null && passwordHasher.VerifyHashMatch(user.PasswordHash, userParam.Password, user.Salt))
            {
                // Create an authentication token
                string token = tokenGenerator.GenerateToken(user.UserId, user.Username, user.Role);

                // Create a ReturnUser object to return to the client
                LoginResponse retUser = new LoginResponse()
                {
                    User = new StandardUser()
                    {
                        UserId = user.UserId,
                        Username = user.Username,
                        Role = user.Role,
                        TeamId = user.TeamId,
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
            try
            {
                IActionResult result;

                StandardUser user = userDAO.AddUser(userParam);

                if (user != null)
                {
                    result = Created(user.Username, user);
                }
                else
                {
                    result = BadRequest(new { message = "An error occurred and user was not created." });
                }

                return result;
            }
            catch (Exception ex)
            {
                return BadRequest("Something wrong with the sql. " + ex.Message);
            }
        }

        [HttpPut]
        public IActionResult UpdateUser(StandardUser user)
        {
            try
            {
                int rowsAffected = userDAO.UpdateUser(user);

                if (rowsAffected != 1)
                    return BadRequest($"Seems that {rowsAffected} rows were changed...");

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Wasn't able to update the user");
            }
        }

        [HttpPut("{userId}")]
        public IActionResult ChangeActive(int userId)
        {
            try
            {
                int rowsAffected = userDAO.SwitchActive(userId);

                if (rowsAffected == 1)
                    return Ok(rowsAffected);

                return BadRequest($"Seems that {rowsAffected} rows were changed...");
            }
            catch (Exception ex)
            {
                return BadRequest("Something errored: " + ex.Message);
            }
        }
    }
}
