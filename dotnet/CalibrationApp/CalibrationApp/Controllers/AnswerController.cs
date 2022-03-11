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

        public AnswerController(IAnswerDAO answerDAO)
        {
            this.dao = answerDAO;
        }

        [HttpPost]
        public ActionResult SubmitAnswer(Answer answer)
        {
            answer.UserId = int.Parse(this.User.FindFirst("sub").Value);
            Answer newAnswer = dao.SubmitAnswer(answer);
            return Created("Answer submitted",newAnswer);
        }

        [HttpGet("participants")]
        public ActionResult GetParticipatingUsers(int calibrationId)
        {
            return Ok(dao.GetParticipatingUsers(calibrationId));
        }

        [HttpGet]
        public ActionResult GetAnswersForCalibration(int calibrationId)
        {
            return Ok(dao.GetAnswersForCalibration(calibrationId));
        }
    }
}
