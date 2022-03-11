using CalibrationApp.Models;

namespace CalibrationApp.DAO
{
    public interface IAnswerDAO
    {
        Answer SubmitAnswer(Answer answer);

        List<Answer> GetAnswersForCalibration(int calibrationId);

        List<User> GetParticipatingUsers(int calibrationId);
    }
}
