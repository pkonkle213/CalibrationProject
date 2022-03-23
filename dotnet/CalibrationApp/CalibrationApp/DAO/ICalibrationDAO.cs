using CalibrationApp.Models;

namespace CalibrationApp.DAO
{
    public interface ICalibrationDAO
    {
        List<ContactType> GetContactTypes();
        Calibration CreateCalibration(Calibration calibration);

        Calibration GetCalibration(int calibrationId, int userId);

        List<Calibration> GetAllCalibrations(int userId); 

        Calibration GetCalibrationResults(int calibrationId);

        void SwitchCalibrationIsOpen(int calibrationId);

        List<Score> GetMyScores(int userId);
    }
}
