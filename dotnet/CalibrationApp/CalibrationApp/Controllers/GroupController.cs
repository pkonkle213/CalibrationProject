using CalibrationApp.DAO;
using CalibrationApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CalibrationApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize]
    public class GroupController : ControllerBase
    {
        private readonly IGroupDAO dao;

        public GroupController(IGroupDAO groupDAO)
        {
            this.dao = groupDAO;        
        }

        /// <summary>
        /// Submits the answers from the group for a specific calibration
        /// </summary>
        /// <param name="answers"></param>
        /// <returns></returns>
        [HttpPost("Answer")]
        public IActionResult SubmitGroupAnswers(List<Answer> answers)
        {
            dao.SubmitAnswers(answers);
            return Ok();
        }

        /// <summary>
        /// Retrieves the answers from the group for a specific calibration
        /// </summary>
        /// <param name="calibrationId"></param>
        /// <returns></returns>
        [HttpGet("{calibrationId}")]
        public IActionResult GetGroupAnswers(int calibrationId)
        {
            List<Answer> answers = dao.GetGroupAnswers(calibrationId);
            return Ok(answers);
        }

        /// <summary>
        /// Updates the score
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        [HttpPut("Score")]
        public IActionResult UpdateScore(Score score)
        {
            dao.UpdateScore(score);
            return Ok();
        }

        /// <summary>
        /// Updates the answer given
        /// </summary>
        /// <param name="answers"></param>
        /// <returns></returns>
        [HttpPut("Answer")]
        public IActionResult UpdateAnswer(List<Answer> answers)
        {
            dao.UpdateAnswers(answers);
            return Ok();
        }
    }
}
