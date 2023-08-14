using CalibrationApp.Models;

namespace CalibrationApp.DAO
{
    public interface IFormDAO
    {
        public List<Form> GetAllForms();
        public List<Form> GetActiveForms();
        public Form GetFormById(int formId);
        public Form CreateNewForm(string newFormName);
        public int UpdateFormName(Form form);
        public int SwitchIsAchivedForm(int formId);
    }
}
