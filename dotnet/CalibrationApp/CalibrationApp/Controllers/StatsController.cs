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
        
        /// <summary>
        /// Retrieves an overall calibrated score
        /// </summary>
        /// <returns></returns>
        [HttpGet("Overall")]
        public IActionResult GetOverallCalibrated()
        {
            int userId = commonFunctions.GetCurrentUserID(User);
            return Ok(dao.GetGeneralCalibrated(userId));
        }

        /// <summary>
        /// Retrieves a list of statistics based upon the questions in all calibrations
        /// </summary>
        /// <returns></returns>
        [HttpGet("Questions")]
        public IActionResult GetQuestionCalibrated()
        {
            int userId = commonFunctions.GetCurrentUserID(User);
            string reason = "Question";
            return Ok(dao.GetCalibrated(userId, reason));
        }

        /// <summary>
        /// Returns a list of statistics based upon the calibrationId
        /// </summary>
        /// <returns></returns>
        [HttpGet("Calibrations")]
        public IActionResult GetCalibrationCalibrated()
        {
            int userId = commonFunctions.GetCurrentUserID(User);
            string reason = "Calibration";
            return Ok(dao.GetCalibrated(userId, reason));
        }

        /// <summary>
        /// Retuns a list of statistics based upon the type of calibrations
        /// </summary>
        /// <returns>A List of Types of Calibrations and Percentage Calibrated</returns>
        [HttpGet("Types")]
        public IActionResult GetTypeCalibrated()
        {
            int userId = commonFunctions.GetCurrentUserID(User);
            string reason = "Type";
            return Ok(dao.GetCalibrated(userId, reason));
        }
    }
}
