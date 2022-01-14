using CalibrationApp.Models;

namespace CalibrationApp.DAO
{
    public interface ICalibrationDAO
    {
        int CreateCalibration(Calibration calibration);

        List<Calibration> GetOpenCalibrations();

        void SwitchCalibrationIsOpen(int calibrationId);
    }
}
