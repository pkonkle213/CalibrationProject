using CalibrationApp.DAO;
using CalibrationApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalibrationApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize]
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerDAO dao;
        private const int userId = 2;

        public AnswerController(IAnswerDAO answerDAO)
        {
            this.dao = answerDAO;
        }

        /// <summary>
        /// Submit a new round of answers
        /// </summary>
        /// <param name="answers">A List of answers</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitAnswers(List<Answer> answers)
        {
            //UserId = int.Parse(this.User.FindFirst("sub").Value);
            dao.SubmitAnswers(answers, userId);
            return Created("Answers submitted!",answers);
        }

        /// <summary>
        /// Updating a round of answers
        /// </summary>
        /// <param name="answers">A list of Answers</param>
        /// <returns></returns>
        [HttpPut]
        public ActionResult UpdateAnswers(List<Answer> answers)
        {
            dao.DeleteAnswers(answers[0].CalibrationId, userId);
            dao.SubmitAnswers(answers, userId);
            return Ok();
        }

        /*
        [HttpGet("participants")]
        public ActionResult GetParticipatingUsers(int calibrationId)
        {
            return Ok(dao.GetParticipatingUsers(calibrationId));
        }
        */
        /*
        [HttpGet]
        public ActionResult GetAnswersForCalibration(int calibrationId)
        {
            return Ok(dao.GetAllAnswersForCalibration(calibrationId));
        }
        */

        /// <summary>
        /// Gets the set of answers for the logged in user
        /// </summary>
        /// <param name="calibrationId">Calibration ID</param>
        /// <returns>List of Answers</returns>
        [HttpGet("{calibrationId}")]
        public ActionResult GetMyAnswers(int calibrationId)
        {
            return Ok(dao.GetMyAnswers(calibrationId));
        }
    }
}
