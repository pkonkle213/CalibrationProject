using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CalibrationApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OptionsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllOptions()
        {
            try
            {
                return Ok("Here's some AnswerOptions for ya!");
            }
            catch (Exception ex)
            {
                return BadRequest("Error in the options controller. " + ex.Message);
            }
        }
    }
}
