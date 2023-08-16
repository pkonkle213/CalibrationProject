using CalibrationApp.DAO;
using CalibrationApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CalibrationApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize]
    public class StatsController : ControllerBase
    {
        private readonly IStatsDAO dao;
        private Common commonFunctions = new Common();

        public StatsController(IStatsDAO statsDAO)
        {
            this.dao = statsDAO;
        }

        [HttpGet("Overall")]
        public IActionResult GetOverallCalibrated()
        {
            int userId = commonFunctions.GetCurrentUserID(User);
            return Ok(dao.GetGeneralCalibrated(userId));
        }

        [HttpGet("Questions")]
        public IActionResult GetQuestionCalibrated(int questionId)
        {
            int userId = commonFunctions.GetCurrentUserID(User);
            string reason = "Question";
            return Ok(dao.GetCalibrated(userId, reason));
        }

        [HttpGet("Calibrations")]
        public IActionResult GetCalibrationCalibrated(int calibrationId)
        {
            int userId = commonFunctions.GetCurrentUserID(User);
            string reason = "Calibration";
            return Ok(dao.GetCalibrated(userId, reason));
        }

        [HttpGet("Types")]
        public IActionResult GetTypeCalibrated(int typeId)
        {
            int userId = commonFunctions.GetCurrentUserID(User);
            string reason = "Type";
            return Ok(dao.GetCalibrated(userId, reason));
        }
    }
}
