using CalibrationApp.Models;

namespace CalibrationApp.DAO
{
    public interface ICalibrationDAO
    {
        List<ContactType> GetContactTypes();
        Calibration CreateCalibration(Calibration calibration);

        Calibration GetCalibration(int calibrationId);

        List<Calibration> GetAllCalibrations(int userId);

        void SwitchCalibrationIsOpen(int calibrationId);

        List<Score> GetMyScores(int userId);
    }
}
