
using CalibrationApp.Models;

namespace CalibrationApp.DAO
{
    public interface IQuestionDAO
    {
        List<Question> GetQuestionsByCalibrationId(int calibrationId);
        List<Question> GetQuestionsByFormId(int formId);
        Question NewQuestion(Question question);
        int UpdateQuestion(Question question);
    }
}
