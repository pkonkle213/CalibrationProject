using Microsoft.AspNetCore.Mvc;
using CalibrationApp.DAO;
using CalibrationApp.Models;

namespace CalibrationApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ParticipantController : ControllerBase
    {
        private readonly IParticipantDAO _dao;

        public ParticipantController(IParticipantDAO dao)
        {
            _dao = dao;
        }

        [HttpGet("Teams")]
        public ActionResult<List<Team>> GetAllTeams()
        {
            try
            {
                List<Team> teams = new List<Team>();
                teams = _dao.GetTeams();
                if (teams == null || teams.Count < 1)
                {
                    return BadRequest("No teams found");
                }

                return Ok(teams);
            }
            catch (Exception ex)
            {
                return BadRequest("Issue with something: " + ex.Message);
            }
        }

        [HttpGet("Roles")]
        public ActionResult<List<Role>> GetAllRoles()
        {
            try
            {
                List<Role> roles = _dao.GetRoles();
                if (roles == null || roles.Count < 1)
                {
                    return BadRequest("No roles found");
                }

                return Ok(roles);
            }
            catch (Exception ex)
            {
                return BadRequest("Issue with something: " + ex.Message);
            }
        }

        [HttpPut("Active")]
        public IActionResult ChangeActive(int userId)
        {
            try
            {
                int rowsAffected = _dao.SwitchActive(userId);
                if (rowsAffected != 1)
                {
                    return BadRequest($"{rowsAffected} were changed");
                }

                return Created("Switched!", userId);
            }
            catch (Exception ex)
            {
                return BadRequest("This is awkward, something didn't change in ParticipantsController/ChangeActive. " + ex.Message);
            }
        }

        [HttpGet("Users")]
        public ActionResult<List<User>> GetAllParticipants()
        {
            try
            {
                List<Participant> participants = _dao.GetAllParticipants();
                if (participants == null || participants.Count < 1)
                {
                    return BadRequest("No participants found");
                }

                return Ok(participants);
            }
            catch (Exception ex)
            {
                return BadRequest("Issue with something: " + ex.Message);
            }
        }
    }
}
