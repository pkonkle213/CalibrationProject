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

        [HttpGet("Overall")]
        public ActionResult<Calibrated> GetOverallCalibrated()
        {
            int userId = GetCurrentUserID();
            string reason = "General";
            return Ok(dao.GetCalibrated(userId, reason, 0));
        }

        [HttpGet("Question")]
        public ActionResult<List<Question>> GetQuestions()
        {
            return Ok(dao.GetAllQuestions());
        }

        [HttpGet("Question/{questionId}")]
        public ActionResult<Calibrated> GetSpecificQuestionCalibrated(int questionId)
        {
            int userId = GetCurrentUserID();
            string reason = "Question";
            return Ok(dao.GetCalibrated(userId, reason, questionId));
        }

        [HttpGet("Calibration/{calibrationId}")]
        public ActionResult<Calibrated> GetSpecificCalibrationCalibrated(int calibrationId)
        {
            int userId = GetCurrentUserID();
            string reason = "Calibration";
            return Ok(dao.GetCalibrated(userId, reason, calibrationId));
        }

        [HttpGet("Type/{typeId}")]
        public ActionResult<Calibrated> GetSpecificTypeCalibrated(int typeId)
        {
            int userId = GetCurrentUserID();
            string reason = "Type";
            return Ok(dao.GetCalibrated(userId, reason, typeId));
        }
    }
}
