using CalibrationApp.Models;

namespace CalibrationApp.DAO
{
    public interface IGroupDAO
    {
        public List<Answer> GetGroupAnswers(int calibrationId);

        public void UpdateScore(Score score);

        public void SubmitAnswers(List<Answer> answers);

        public void UpdateAnswers(List<Answer> answers);
    }
}
