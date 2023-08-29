using CalibrationApp.DAO;
using CalibrationApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CalibrationApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize]
    public class CalibrationController : ControllerBase
    {
        private readonly ICalibrationDAO dao;
        private Common commonFunctions = new Common();

        public CalibrationController(ICalibrationDAO calibrationDAO)
        {
            dao = calibrationDAO;
        }

        [HttpGet("Types")]
        public ActionResult<List<ContactType>> GetContactTypes()
        {
            try
            {
                return Ok(dao.GetContactTypes());
            }
            catch (Exception ex)
            {
                return BadRequest("Don't look at me! " + ex);
            }
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public ActionResult<Calibration> CreateCalibration(Calibration calibration)
        {
            try
            {
                return Created("Calibration created!", dao.CreateCalibration(calibration));
            }
            catch (Exception ex)
            {
                return BadRequest("Could not create calibration: " + ex.Message);
            }
        }


        /// <summary>
        /// Gets a specific calibration by its CalibrationID number
        /// </summary>
        /// <param name="calibrationId">The CalibrationID you're looking for</param>
        /// <returns>A single calibration</returns>
        [HttpGet]
        [Route("{calibrationId}")]
        public ActionResult<Calibration> GetSingleCalibration(int calibrationId)
        {
            try
            {
                Calibration calibration = dao.GetCalibration(calibrationId);
                if (calibration == null)
                {
                    return BadRequest("No calibration found");
                }
                else
                {
                    return Ok(calibration);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("I have no idea. " + ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all calibrations in reverse chronological order
        /// </summary>
        /// <returns>All calibrations</returns>
        [HttpGet]
        [Route("All")]
        public ActionResult<List<Calibration>> GetAllCalibrations()
        {
            try
            {
                List<Calibration> calibrations = dao.GetAllCalibrations();
                if (calibrations == null || calibrations.Count == 0)
                {
                    return BadRequest("No calibrations found");
                }
                else
                {
                    return Ok(calibrations);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Something went wrong, obviously. " + ex.Message);
            }
        }

        /// <summary>
        /// Updates the IsOpen category of a calibration to prevent further edits
        /// </summary>
        /// <param name="calibrationId">The ID associated with the calibration</param>
        /// <returns>Confirmation that it was successful</returns>
        [HttpPut("{calibrationId}")]
        public ActionResult SwitchCalibrationIsOpen(int calibrationId)
        {
            try
            {
                var rowsAffected = dao.SwitchCalibrationIsOpen(calibrationId);

                if (rowsAffected != 1)
                    return BadRequest($"Unfortunately, {rowsAffected} rows were affected.");

                return Ok(rowsAffected);
            }
            catch (Exception ex)
            {
                return BadRequest("Bad news, Jim. " + ex.Message);
            }
        }

        [HttpGet("Scores")]
        public ActionResult GetMyScores()
        {
            try
            {
                int userId = commonFunctions.GetCurrentUserID(User);
                return Ok(dao.GetMyScores(userId));
            }
            catch (Exception ex)
            {
                return BadRequest("This ain't it. " + ex.Message);
            }
        }
    }
}