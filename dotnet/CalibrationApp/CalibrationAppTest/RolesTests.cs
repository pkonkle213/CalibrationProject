using CalibrationApp.DAO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CalibrationAppTest
{
    [TestClass]
    public class ReservationDAOTest : TestBase
    {
        [TestMethod]
        public void GetTeamsTest()
        {
            //Arrange
            SqlUserDAO dao = new SqlUserDAO(ConnectionString);

            //Act
           var teams = dao.GetTeams();

            //Assert
            Assert.IsTrue(teams != null);
            Assert.IsTrue(teams.Count == 5);
        }
    }
}
