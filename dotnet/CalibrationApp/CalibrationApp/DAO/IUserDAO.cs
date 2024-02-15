using System.Collections.Generic;
using CalibrationApp.Models;

namespace CalibrationApp.DAO
{
    public interface IUserDAO
    {
        public SaltedUser GetUser(string username);
        public StandardUser AddUser(RegisterUser user);
        public List<StandardUser> GetAllUsers();
        public StandardUser UpdateUser(StandardUser user);
        public void SwitchActive(int userId);
    }
}
