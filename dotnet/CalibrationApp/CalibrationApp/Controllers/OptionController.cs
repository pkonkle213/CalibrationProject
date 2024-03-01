using CalibrationApp.DAO;
using CalibrationApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CalibrationApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OptionController : ControllerBase
    {
        private readonly IOptionDAO dao;
        private Common commonFunctions = new Common();

        public OptionController(IOptionDAO calibrationDAO)
        {
            dao = calibrationDAO;
        }

        /// <summary>
        /// Gets all options for a form
        /// </summary>
        /// <param name="formId">The form ID for the options</param>
        /// <returns>A list of options</returns>
        [HttpGet]
        [Route("{formId}")]
        public IActionResult GetOptions(int formId)
        {
            try
            {
                var options = dao.GetAllOptions(formId);
                if (options != null)
                    return Ok(options);

                return BadRequest("No options found");
            }
            catch (Exception ex)
            {
                return BadRequest("Error found while getting all options: " + ex.Message);
            }
        }

        /// <summary>
        /// Creates a new option for a form
        /// </summary>
        /// <param name="option">The new option</param>
        /// <returns>The option with an optionId</returns>
        [HttpPost]
        public IActionResult CreateNewOption(Option option)
        {
            try
            {
                option = dao.CreateNewOption(option);
                if (option.Id != null)
                {
                    return Ok(option);
                }

                return BadRequest("Option was not given an ID");
            }
            catch (Exception ex)
            {
                return BadRequest("Error while attempting to create a new option: " + ex.Message);
            }
        }

        /// <summary>
        /// Updates the given option
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult UpdateOption (Option option)
        {
            return null;
        }
    }
}
