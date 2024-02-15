using CalibrationApp.DAO;
using CalibrationApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CalibrationApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize]
    public class FormController : ControllerBase
    {
        private readonly IFormDAO _dao;

        public FormController(IFormDAO dao)
        {
            _dao = dao;
        }

        /// <summary>
        /// Retrieves all active forms for creating a new calibration
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Active")]
        public IActionResult GetActiveForms()
        {
            try
            {
                var forms = _dao.GetActiveForms();
                if (forms != null)
                    return Ok(forms);

                return BadRequest("No forms found");
            }
            catch (Exception ex)
            {
                return BadRequest("I don't know. " + ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all forms
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("All")]
        public IActionResult GetAllForms()
        {
            try
            {
                var forms = _dao.GetAllForms();
                if (forms != null)
                    return Ok(forms);

                return BadRequest("No forms found");
            }
            catch (Exception ex)
            {
                return BadRequest("I don't know. " + ex.Message);
            }
        }

        /// <summary>
        /// Creates a new form
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateNewForm(Form form)
        {
            try
            {
                var formName = form.FormName;
                var newForm = _dao.CreateNewForm(formName);
                return Created("New form has been created", newForm);
            }
            catch (Exception ex)
            {
                return BadRequest("Something something " + ex.Message);
            }
        }

        /// <summary>
        /// Switches if a form is archived
        /// </summary>
        /// <param name="formId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Disable/{formId}")]
        public IActionResult SwitchIsArchivedForm(int formId)
        {
            try
            {
                var rowsAffected = _dao.SwitchIsArchivedForm(formId);

                if (rowsAffected == 1)
                {
                    return Created("That worked!", rowsAffected);
                }
                else
                {
                    return BadRequest($"{rowsAffected} rows were changed, which is an issue");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Something didn't work right: " + ex.Message);
            }
        }

        /// <summary>
        /// Updates the entire form
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateName")]
        public IActionResult UpdateForm(Form form)
        {
            try
            {
                int rowsAffected = _dao.UpdateFormName(form);

                if (rowsAffected == 1)
                    return Created("Updated the form", form);

                return BadRequest($"{rowsAffected} rows were affected, which wasn't 1");

            }
            catch (Exception ex)
            {
                return BadRequest("Well don't you look stupid. " + ex.Message);
            }
        }
    }
}
