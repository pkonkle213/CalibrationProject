using System.Collections.Generic;
using CalibrationApp.Models;

namespace CalibrationApp.DAO
{
    public interface IUserDAO
    {
        List<Team> GetTeams();
        List<Role> GetRoles();
        User GetUser(string username);
        User AddUser(string username, string password, string role, bool isActive, string team, string firstName, string lastName);
        List<User> GetAllUsers();
    }
}
