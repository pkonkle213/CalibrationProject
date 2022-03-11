using CalibrationApp.DAO;
using CalibrationApp.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalibrationApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[Authorize]
    public class CalibrationController : ControllerBase
    {
        private readonly ICalibrationDAO dao;

        public CalibrationController(ICalibrationDAO calibrationDAO)
        {
            this.dao = calibrationDAO;
        }

        /* 
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public ActionResult<Calibration> CreateCalibration(Calibration calibration)
        {
            try
            {
                return Created("Calibration created!", dao.CreateCalibration(calibration.CalibrationDate, calibration.TypeId, calibration.ContactId, calibration.RepFirstName, calibration.RepLastName));
            }
            catch (Exception ex)
            {
                return BadRequest("Could not create calibration: " + ex.Message);
            }
        }
        */

        /// <summary>
        /// Gets a specific calibration by its CalibrationID number
        /// </summary>
        /// <param name="calibrationId">The CalibrationID you're looking for</param>
        /// <returns>A single calibration</returns>
        [HttpGet]
        [Route("{calibrationId}")]
        public ActionResult<Calibration> GetSpecificCalibrations(int calibrationId)
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

        /// <summary>
        /// Retrieves all calibrations in reverse chronological order
        /// </summary>
        /// <returns>All calibrations</returns>
        [HttpGet]
        [Route("All")]
        public ActionResult<List<Calibration>> GetAllCalibrations()
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

        /// <summary>
        /// Updates the IsOpen category of a calibration to prevent further edits
        /// </summary>
        /// <param name="calibrationId">The ID associated with the calibration</param>
        /// <returns>Confirmation that it was successful</returns>
        [HttpPut("{calibrationId}")]
        public ActionResult SwitchCalibrationIsOpen(int calibrationId)
        {
            dao.SwitchCalibrationIsOpen(calibrationId);
            return Ok("Switched!");
        }
    }
}
