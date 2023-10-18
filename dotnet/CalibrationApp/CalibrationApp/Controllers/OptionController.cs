using CalibrationApp.DAO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CalibrationApp.Controllers
{
    [Route("api/[controller]")]
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
        [Route("Option/{formId}")]
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
    }
}
