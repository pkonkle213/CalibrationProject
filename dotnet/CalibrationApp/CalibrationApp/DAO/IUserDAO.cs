﻿using System.Collections.Generic;
using CalibrationApp.Models;

namespace CalibrationApp.DAO
{
    public interface IUserDAO
    {
        User GetUser(string username);
        User AddUser(string username, string password, string role, string team, string firstName, string lastName);
    }
}
