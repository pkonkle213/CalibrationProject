
using CalibrationApp.Models;

namespace CalibrationApp.DAO
{
    public interface IQuestionDAO
    {
        public List<Question> GetQuestionsByCalibrationId(int calibrationId);
        public List<Question> GetQuestionsByFormId(int formId);
        public Question NewQuestion(Question question);
        public int UpdateQuestion(Question question);
        public List<Option> GetAllOptions();
    }
}
