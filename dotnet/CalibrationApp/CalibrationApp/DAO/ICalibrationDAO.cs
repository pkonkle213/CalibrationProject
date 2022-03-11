using CalibrationApp.Models;

namespace CalibrationApp.DAO
{
    public interface ICalibrationDAO
    {
        // Calibration CreateCalibration(DateTime date, int type, string contactId, string first, string last);

        Calibration GetCalibration(int calibrationId);

        List<Calibration> GetAllCalibrations(); 

        Calibration GetCalibrationResults(int id);

        void SwitchCalibrationIsOpen(int calibrationId);
    }
}
