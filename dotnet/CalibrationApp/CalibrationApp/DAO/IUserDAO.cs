using System.Collections.Generic;
using CalibrationApp.Models;

namespace CalibrationApp.DAO
{
    public interface IUserDAO
    {
        User GetUser(string username);
        User AddUser(string username, string password, int role_id, int team_id, string firstName, string lastName);
    }
}
