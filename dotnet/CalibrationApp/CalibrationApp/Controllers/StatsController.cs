using CalibrationApp.DAO;
using CalibrationApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalibrationApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class StatsController : ControllerBase
    {
        private readonly IStatsDAO dao;

        public StatsController(IStatsDAO statsDAO)
        {
            this.dao = statsDAO;
        }

        private int GetCurrentUserID()
        {
            var user = this.User;
            int id = 0;
            if (user.Identity.Name != null)
            {
                var idClaim = user.FindFirst("sub");
                string idString = idClaim.Value;
                id = int.Parse(idString);
            }
            return id;
        }

        [HttpGet("Question")]
        public ActionResult<List<Question>> GetQuestions()
        {
            return Ok(dao.GetAllQuestions());
        }

        [HttpGet("Individual")]
        public ActionResult<Calibrated> GetOverallCalibrated()
        {
            int userId = GetCurrentUserID();
            return Ok(dao.GetCalibrated(userId,0));
        }

        [HttpGet("Individual/{calibrationId}")]
        public ActionResult<Calibrated> GetSpecificCalibrated(int calibrationId)
        {
            int userId = GetCurrentUserID();
            return Ok(dao.GetCalibrated(userId,calibrationId));
        }
    }
}
