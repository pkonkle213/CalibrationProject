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
        private const int userId = 2; //Only needed until I can use a logged in user

        public AnswerController(IAnswerDAO answerDAO)
        {
            this.dao = answerDAO;
        }

        /// <summary>
        /// Submit answers for a calibration and a user
        /// </summary>
        /// <param name="answers">A List of answers</param>
        /// <returns></returns>
        [HttpPost("Answer")]
        public ActionResult SubmitAnswers(List<Answer> answers)
        {
            //UserId = int.Parse(this.User.FindFirst("sub").Value);
            dao.SubmitAnswers(answers, userId);
            return Created("Answers submitted!",answers);
        }

        [HttpPost("Score")]
        public ActionResult SubmitScore(Score score)
        {
            dao.SubmitScore(score, userId);
            return Ok();
        }

        /// <summary>
        /// Updating a round of answers
        /// </summary>
        /// <param name="answers">A list of Answers</param>
        /// <returns></returns>
        [HttpPut("Answer")]
        public ActionResult UpdateAnswers(List<Answer> answers)
        {
            dao.DeleteAnswers(answers[0].CalibrationId, userId);
            dao.SubmitAnswers(answers, userId);
            return Ok();
        }

        [HttpPut("Score")]
        public ActionResult UpdateScore(Score score)
        {
            dao.DeleteScore(score, userId);
            dao.SubmitScore(score, userId);
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
            int userId = 2;
            return Ok(dao.GetMyAnswers(calibrationId, userId));
        }
    }
}
