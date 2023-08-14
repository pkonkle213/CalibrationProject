using CalibrationApp.DAO;
using CalibrationApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CalibrationApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
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
        [Route("Calibration/{calibrationId}")]
        public ActionResult GetQuestionsByCalibrationId(int calibrationId)
        {
            try
            {
                return Ok(dao.GetQuestionsByCalibrationId(calibrationId));
            }
            catch (Exception ex)
            {
                return BadRequest("Well that certainly didn't work. " + ex.Message);
            }
        }

        /// <summary>
        /// Gets all questions based on the form
        /// </summary>
        /// <param name="formId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Form/{formId}")]
        public ActionResult GetQuestionsByFormId(int formId)
        {
            try
            {
                return Ok(dao.GetQuestionsByFormId(formId));
            }
            catch (Exception ex)
            {
                return BadRequest("Well don't look at me! " + ex.Message);
            }
        }

        [HttpPut]
        public IActionResult UpdateAllQuestions(IEnumerable<Question> questions)
        {
            int rowsAffected = 0;
            try
            {
                foreach (Question question in questions)
                    rowsAffected += dao.UpdateQuestion(question);
                if (rowsAffected != questions.Count())
                    return BadRequest($"{rowsAffected} rows affected instead of {questions.Count()}");
                return Created("Updated!", questions);
            }
            catch (Exception ex)
            {
                return BadRequest("NOT TODAY, SUCKA! " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult NewQuestion(Question question)
        {
            try
            {
                Question newQuestion = dao.NewQuestion(question);
                return Ok(newQuestion);
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong, obviously: " + ex.Message);
            }
        }
    }
}
