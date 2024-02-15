using CalibrationApp.DAO;
using CalibrationApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CalibrationApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamDAO dao;

        public TeamsController(ITeamDAO dao)
        {
            this.dao = dao;
        }

        /// <summary>
        /// Returns all Teams
        /// </summary>
        /// <returns>A List of Teams</returns>
        [HttpGet]
        public IActionResult GetAllTeams()
        {
            return Ok(dao.GetTeams());
        }
    }
}
