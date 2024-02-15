using CalibrationApp.DAO;
using CalibrationApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CalibrationApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionDAO questionDAO;
        private readonly IOptionDAO optionDAO;

        public QuestionController(IOptionDAO optionDAO, IQuestionDAO questionDAO)
        {
            this.optionDAO = optionDAO;
            this.questionDAO = questionDAO;
        }

        /// <summary>
        /// Gets all questions linked to the calibration
        /// </summary>
        /// <param name="calibrationId">The Calibration ID for the questions</param>
        /// <returns>A list of Questions</returns>
        [HttpGet]
        [Route("Calibration/{calibrationId}")]
        public IActionResult GetQuestionsByCalibrationId(int calibrationId)
        {
            try
            {
                var questions = questionDAO.GetQuestionsByCalibrationId(calibrationId);

                foreach(Question question in questions)
                {
                    question.Options = optionDAO.GetEnabledOptionsForQuestion(question.Id);
                }

                return Ok(questions);
            }
            catch (Exception ex)
            {
                return BadRequest("Well that certainly didn't work. " + ex.Message);
            }
        }

        /// <summary>
        /// Gets all questions based on the form
        /// </summary>
        /// <param name="formId">The form ID of the questions</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Form/{formId}")]
        public IActionResult GetQuestionsByFormId(int formId)
        {
            try
            {
                var questions = questionDAO.GetQuestionsByFormId(formId);

                foreach (Question question in questions)
                {
                    question.Options = optionDAO.GetEnabledOptionsForQuestion(question.Id);
                }

                return Ok(questions);
            }
            catch (Exception ex)
            {
                return BadRequest("Well don't look at me! " + ex.Message);
            }
        }

        /// <summary>
        /// Updates all questions at once
        /// </summary>
        /// <param name="questions">A list of questions to update</param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult UpdateAllQuestions(IEnumerable<Question> questions)
        {
            try
            {
                foreach (Question question in questions)
                {
                    questionDAO.UpdateQuestion(question);
                }
                
                return Created("Updated!", questions);
            }
            catch (Exception ex)
            {
                return BadRequest("NOT TODAY, SUCKA! " + ex.Message);
            }
        }

        /// <summary>
        /// Creatres a new question
        /// </summary>
        /// <param name="question">The question to create</param>
        /// <returns>A question with a valid ID</returns>
        [HttpPost]
        public IActionResult NewQuestion(Question question)
        {
            try
            {
                return Ok(questionDAO.NewQuestion(question));
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong, obviously: " + ex.Message);
            }
        }
      
        /// <summary>
        /// Get questions in an editable list by formId
        /// </summary>
        /// <param name="formId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Edit/{formId}")]
        public IActionResult GetEditQuestions(int formId)
        {
            try
            {
                var questions = questionDAO.GetEditQuestionsByForm(formId);

                if (questions != null)
                {
                    return Ok(questions);
                }

                return BadRequest("No questions found");
            }
            catch (Exception ex)
            {
                return BadRequest("Error found while getting all questions: " + ex.Message);
            }
        }

    }
}
