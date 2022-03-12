using CalibrationApp.Models;

namespace CalibrationApp.DAO
{
    public interface IAnswerDAO
    {
        /*
        Answer SubmitAnswer(Answer answer);
        */
        /*
        List<Answer> GetAllAnswersForCalibration(int calibrationId);
        */
        /*
        List<User> GetParticipatingUsers(int calibrationId);
        */

        void SubmitAnswers(List<Answer> answers, int userId);
        List<Answer> GetMyAnswers(int calibrationId);
        void DeleteAnswers(int calibrationId, int userId);
    }
}
