using CalibrationApp.DAO;
using CalibrationApp.Models;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost("Answer")]
        public ActionResult SubmitGroupAnswers(List<Answer> answers)
        {
            dao.SubmitAnswers(answers);
            return Ok();
        }

        [HttpGet("{calibrationId}")]
        public ActionResult<List<Answer>> GetGroupAnswers(int calibrationId)
        {
            List<Answer> answers = dao.GetGroupAnswers(calibrationId);
            return Ok(answers);
        }

        [HttpPut("Score")]
        public ActionResult UpdateScore(Score score)
        {
            dao.UpdateScore(score);
            return Ok();
        }

        [HttpPut("Answer")]
        public ActionResult UpdateAnswer(List<Answer> answers)
        {
            dao.UpdateAnswers(answers);
            return Ok();
        }
    }
}
