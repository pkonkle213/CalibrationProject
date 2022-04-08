using CalibrationApp.Models;

namespace CalibrationApp.DAO
{
    public interface IStatsDAO
    {
        public List<Question> GetAllQuestions();

        public List<Answer> GetMyAnswers(int userId);

        public List<Answer> GetGroupAnswers();
    }
}
