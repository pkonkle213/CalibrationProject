using CalibrationApp.DAO;
using CalibrationApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalibrationApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionDAO dao;

        public QuestionController(IQuestionDAO questionDAO)
        {
            this.dao = questionDAO;
        }

        /// <summary>
        /// Gets all questions linked to the calibration
        /// </summary>
        /// <param name="calibrationId">Calibration ID</param>
        /// <returns>A list of Questions</returns>
        [HttpGet]
        [Route("{calibrationId}")]
        public ActionResult GetQuestions(int calibrationId)
        {
            return Ok(dao.GetQuestions(calibrationId));
        }
    }
}
