using CalibrationApp.DAO;
using CalibrationApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalibrationApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerDAO dao;

        public AnswerController(IAnswerDAO answerDAO)
        {
            this.dao = answerDAO;
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

        /// <summary>
        /// Submit answers for a calibration and a user
        /// </summary>
        /// <param name="answers">A List of answers</param>
        /// <returns></returns>
        [HttpPost("Answer")]
        public ActionResult SubmitAnswers(List<Answer> answers)
        {
            int userId = GetCurrentUserID();
            int count = dao.SubmitAnswers(answers, userId);
            if (count==1)
            {
                return Created("Answers submitted!", answers);
            }
            else
            {
                return BadRequest("Something went wrong");
            }
        }

        [HttpPost("Score")]
        public ActionResult SubmitScore(Score score)
        {
            int userId = GetCurrentUserID();
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
            int userId = GetCurrentUserID();
            dao.UpdateAnswers(answers, userId);
            return Ok();
        }

        [HttpPut("Score")]
        public ActionResult UpdateScore(Score score)
        {
            int userId = GetCurrentUserID();
            dao.UpdateScore(score, userId);
            return Ok();
        }


        [HttpGet("Participants/{calibrationId}")]
        public ActionResult GetParticipatingUsers(int calibrationId)
        {
            return Ok(dao.GetParticipatingUsers(calibrationId));
        }

        [HttpGet("Group/{calibrationId}")]
        public ActionResult GetAnswersForCalibration(int calibrationId)
        {
            int userId = 0;
            return Ok(dao.GetMyAnswers(calibrationId, userId));
        }
        

        /// <summary>
        /// Gets the set of answers for the logged in user
        /// </summary>
        /// <param name="calibrationId">Calibration ID</param>
        /// <returns>List of Answers</returns>
        [HttpGet("{calibrationId}")]
        public ActionResult GetMyAnswers(int calibrationId)
        {
            int userId = GetCurrentUserID();
            return Ok(dao.GetMyAnswers(calibrationId, userId));
        }
    }
}
