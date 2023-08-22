using System.Collections.Generic;
using CalibrationApp.Models;

namespace CalibrationApp.DAO
{
    public interface IUserDAO
    {
        List<Team> GetTeams();
        SaltedUser GetUser(string username);
        StandardUser AddUser(RegisterUser user);
        List<StandardUser> GetAllUsers();
        int UpdateUser(StandardUser user);
        int SwitchActive(int userId);
    }
}
