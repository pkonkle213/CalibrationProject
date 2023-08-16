using CalibrationApp.Models;

namespace CalibrationApp.DAO
{
    public interface IStatsDAO
    {
        public Statistic GetGeneralCalibrated(int userId);
        public List<Statistic> GetCalibrated(int userId, string reason);
    }
}
