using CalibrationApp.Models;

namespace CalibrationApp.DAO
{
    public interface IStatsDAO
    {
        public List<Question> GetAllQuestions();

        public Calibrated GetCalibrated(int userId, string reason, int elementId);
    }
}
