using CalibrationApp.Models;

namespace CalibrationApp.DAO
{
    public interface IOptionDAO
    {
        public List<Option> GetAllOptions(int formId);
        public List<Option> GetEnabledOptionsForQuestion(int questionId);
        public Option CreateNewOption(Option option);
    }
}
