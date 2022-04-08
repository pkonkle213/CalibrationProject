using CalibrationApp.Models;

namespace CalibrationApp.DAO
{
    public interface IIndividualDAO
    {
        List<User> GetParticipatingUsers(int calibrationid);
        List<Answer> GetMyAnswers(int calibrationId, int userId);
        int SubmitAnswers(List<Answer> answers, int userId);
        void SubmitScore(Score score, int userId);
        int UpdateAnswers(List<Answer> answers, int userId);
        void UpdateScore(Score score, int userId);
    }
}
