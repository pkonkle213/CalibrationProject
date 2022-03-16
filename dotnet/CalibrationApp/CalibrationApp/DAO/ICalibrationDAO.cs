using CalibrationApp.Models;

namespace CalibrationApp.DAO
{
    public interface ICalibrationDAO
    {
        // Calibration CreateCalibration(DateTime date, int type, string contactId, string first, string last);

        Calibration GetCalibration(int calibrationId, int userId);

        List<Calibration> GetAllCalibrations(int userId); 

        Calibration GetCalibrationResults(int calibrationId);

        void SwitchCalibrationIsOpen(int calibrationId);
    }
}
