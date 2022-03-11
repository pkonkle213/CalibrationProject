using CalibrationApp.DAO;
using CalibrationApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalibrationApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionDAO dao;

        public QuestionController(IQuestionDAO questionDAO)
        {
            this.dao = questionDAO;
        }

        [HttpGet]
        public ActionResult GetQuestions(int calibrationId)
        {
            return Ok(dao.GetQuestions(calibrationId));
        }

        [HttpPost]
        public ActionResult SubmitAnswers(List<Answer> answers)
        {
            dao.SubmitAnswers(answers);
            return Ok();
        }
    }
}
