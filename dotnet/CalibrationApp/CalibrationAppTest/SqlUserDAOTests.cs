using CalibrationApp.DAO;
using CalibrationApp.Models;

namespace CalibrationAppTest
{
    [TestClass]
    public class SqlUserDAOTests : TestBase
    {
        [TestMethod]
        public void GetUserShouldReturnValidUser()
        {
            SqlUserDAO dao = new SqlUserDAO(ConnectionString);
            var testUser = new SaltedUser()
            {
                UserId = 1,
                Username = "sadmin",
                FirstName = "System",
                LastName = "Admin",
                Role = "Admin",
                TeamId = 1,
                IsActive = true,
            };

            var result = dao.GetUser(testUser.Username);

            Assert.IsNotNull(result);
            Assert.AreEqual(testUser.UserId, result.UserId);
            Assert.AreEqual(testUser.Username, result.Username);
            Assert.AreEqual(testUser.LastName, result.LastName);
            Assert.AreEqual(testUser.FirstName, result.FirstName);
            Assert.AreEqual(testUser.Role, result.Role);
            Assert.AreEqual(testUser.TeamId, result.TeamId);
            Assert.AreEqual(testUser.IsActive, result.IsActive);
        }

        [TestMethod]
        public void GetAllUsersShouldReturnListOfUsers()
        {
            SqlUserDAO dao = new SqlUserDAO(ConnectionString);

            var results = dao.GetAllUsers();

            Assert.IsNotNull(results);
            Assert.AreEqual(3, results.Count);
        }

        [TestMethod]
        public void UpdateUserChangesInformation()
        {
            var dao = new SqlUserDAO(ConnectionString);

            var updateUser = new StandardUser()
            {
                UserId = 1,
                Username = "pkonkle",
                Role = "Participant",
                TeamId = 1,
                FirstName = "Phillip",
                LastName = "Konkle"
            };

            var result = dao.UpdateUser(updateUser);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddUserSuccessfullyAddsUser()
        {
            var dao = new SqlUserDAO(ConnectionString);
            var testUser = new RegisterUser()
            {
                Username = "pkonkle",
                Password = "password",
                Role = "Admin",
                TeamId = 1,
                FirstName = "Phillip",
                LastName = "Konkle"
            };

            var result = dao.AddUser(testUser);
            var added = dao.GetUser(testUser.Username);

            Assert.IsNotNull(result);
            Assert.IsNotNull(added);
            Assert.AreEqual(testUser.Username, result.Username);
            Assert.AreEqual(testUser.FirstName, result.FirstName);
            Assert.AreEqual(testUser.LastName, result.LastName);
            Assert.AreEqual(testUser.Role, result.Role);
            Assert.AreEqual(testUser.TeamId, result.TeamId);
        }

        [TestMethod]
        [DataRow(1,"sadmin",false)]
        [DataRow(3,"ouser",true)]
        public void SwitchActiveWorks(int userId, string username, bool expected)
        {
            var dao = new SqlUserDAO(ConnectionString);

            dao.SwitchActive(userId);
            var result = dao.GetUser(username);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.IsActive);
        }
    }
}
