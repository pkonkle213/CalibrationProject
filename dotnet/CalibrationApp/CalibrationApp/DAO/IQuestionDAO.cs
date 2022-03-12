using CalibrationApp.Models;

namespace CalibrationApp.DAO
{
    public interface IQuestionDAO
    {
        List<Question> GetQuestions(int calibrationId);

        List<Option> GetOptions(int questionId, bool isCategory);
    }
}
