using CalibrationApp.DAO;

namespace CalibrationAppTest
{
    [TestClass]
    public class SqlQuestionDAOTests : TestBase
    {
        [TestMethod]
        public void GetEditQuestionsReturnsCorrectInformation()
        {
            SqlQuestionDAO dao = new SqlQuestionDAO(ConnectionString);
            int formId = 1;

            var questions = dao.GetEditQuestionsByForm(formId);

            Assert.IsNotNull(questions);
        }
    }
}
