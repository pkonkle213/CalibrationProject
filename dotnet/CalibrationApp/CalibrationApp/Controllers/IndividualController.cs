using CalibrationApp.DAO;
using CalibrationApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CalibrationApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize]
    public class IndividualController : ControllerBase
    {
        private readonly IIndividualDAO dao;
        private Common commonFunctions = new Common();

        public IndividualController(IIndividualDAO indivDAO)
        {
            this.dao = indivDAO;
        }

        /// <summary>
        /// Submit answers for a calibration and a user
        /// </summary>
        /// <param name="answers">A List of answers</param>
        /// <returns></returns>
        [HttpPost("Answer")]
        public IActionResult SubmitAnswers(List<Answer> answers)
        {
            int userId = commonFunctions.GetCurrentUserID(User);
            int count = dao.SubmitAnswers(answers, userId);
            if (count == 1)
            {
                return Created("Answers submitted!", answers);
            }
            else
            {
                return BadRequest("Something went wrong");
            }
        }

        /// <summary>
        /// Submits a score from a logged in user
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        [HttpPost("Score")]
        public IActionResult SubmitScore(Score score)
        {
            int userId = commonFunctions.GetCurrentUserID(User);
            dao.SubmitScore(score, userId);
            return Ok();
        }

        /// <summary>
        /// Updating a round of answers
        /// </summary>
        /// <param name="answers">A list of Answers</param>
        /// <returns></returns>
        [HttpPut("Answer")]
        public IActionResult UpdateAnswers(List<Answer> answers)
        {
            int userId = commonFunctions.GetCurrentUserID(User);
            dao.UpdateAnswers(answers, userId);
            return Ok();
        }

        /// <summary>
        /// Retrieves scores for the logged in user
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        [HttpPut("Score")]
        public IActionResult UpdateScore(Score score)
        {
            int userId = commonFunctions.GetCurrentUserID(User);
            dao.UpdateScore(score, userId);
            return Ok();
        }

        /// <summary>
        /// Retrieves all participants of a specific calibration
        /// </summary>
        /// <param name="calibrationId">The calibration ID</param>
        /// <returns></returns>
        [HttpGet("Participants/{calibrationId}")]
        public IActionResult GetParticipatingUsers(int calibrationId)
        {
            return Ok(dao.GetParticipatingUsers(calibrationId));
        }

        /// <summary>
        /// Gets the set of answers for the logged in user
        /// </summary>
        /// <param name="calibrationId">Calibration ID</param>
        /// <returns>List of Answers</returns>
        [HttpGet("{calibrationId}")]
        public IActionResult GetMyAnswers(int calibrationId)
        {
            int userId = commonFunctions.GetCurrentUserID(User);
            return Ok(dao.GetMyAnswers(calibrationId, userId));
        }
    }
}
