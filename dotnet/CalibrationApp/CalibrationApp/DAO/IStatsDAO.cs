using CalibrationApp.Models;

namespace CalibrationApp.DAO
{
    public interface IStatsDAO
    {
        public Calibrated GetCalibrated(int userId, string reason);
    }
}
