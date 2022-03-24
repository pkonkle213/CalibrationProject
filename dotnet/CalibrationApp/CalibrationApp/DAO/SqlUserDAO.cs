using System;
using System.Data.SqlClient;
using CalibrationApp.Models;
using CalibrationApp.Security;
using CalibrationApp.Security.Models;

namespace CalibrationApp.DAO
{
    public class SqlUserDAO : IUserDAO
    {

        private string sqlGetUser = "SELECT u.user_id, u.username, u.password_hash, u.salt, r.role_name, t.team_name, u.first_name, u.last_name " +
            "FROM Users u " +
            "INNER JOIN Roles r ON r.role_id=u.role_id " +
            "INNER JOIN Teams t ON t.team_id=u.team_id " +
            "WHERE u.username = @username";

        private string sqlAddUser = "INSERT INTO Users (username, password_hash, salt, first_name, last_name, role_id, team_id) VALUES " +
            "(@username, @password_hash, @salt, @first_name, @last_name, @role_id, @team_id)";

        private readonly string connectionString;
        public SqlUserDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public User GetUser(string username)
        {
            User returnUser = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(sqlGetUser, conn))
                {
                    command.Parameters.AddWithValue("@username", username);
                    
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows && reader.Read())
                    {
                        returnUser = GetUserFromReader(reader);
                    }
                }
            }

            return returnUser;
        }

        public List<Team> GetTeams()
        {
            List<Team> teams = new List<Team>();
            string sql = "SELECT team_id, team_name " +
                         "FROM Teams";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Team team = new Team();

                            team.Id = Convert.ToInt32(reader["team_id"]);
                            team.Name = Convert.ToString(reader["team_name"]);

                            teams.Add(team);
                        }
                    }
                }
            }

            return teams;
        }
        
        public List<Role> GetRoles()
        {
            List<Role> roles = new List<Role>();
            string sql = "SELECT role_id, role_name " +
                         "FROM Roles";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Role role = new Role();

                            role.Id = Convert.ToInt32(reader["role_id"]);
                            role.Name = Convert.ToString(reader["role_name"]);

                            roles.Add(role);
                        }
                    }
                }
            }

            return roles;
        }

        private int GetRoleId(string roleString)
        {
            List<Role> roles = GetRoles();

            foreach (Role role in roles)
            {
                if (role.Name == roleString)
                {
                    return role.Id;
                }
            }

            return -1;
        }

        private int GetTeamId(string teamString)
        {
            List<Team> teams = GetTeams();

            foreach (Team team in teams)
            {
                if (team.Name == teamString)
                {
                    return team.Id;
                }
            }

            return -1;
        }

        public User AddUser(string username, string password, string role, string team, string firstName, string lastName)
        {
            IPasswordHasher passwordHasher = new PasswordHasher();
            PasswordHash hash = passwordHasher.ComputeHash(password);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sqlAddUser, conn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password_hash", hash.Password);
                cmd.Parameters.AddWithValue("@salt", hash.Salt);
                cmd.Parameters.AddWithValue("@first_name", firstName);
                cmd.Parameters.AddWithValue("@last_name", lastName);
                cmd.Parameters.AddWithValue("@role_id", GetRoleId(role));
                cmd.Parameters.AddWithValue("@team_id", GetTeamId(team));
                cmd.ExecuteNonQuery();
            }

            return GetUser(username);
        }

        private User GetUserFromReader(SqlDataReader reader)
        {
            User u = new User()
            {
                UserId = Convert.ToInt32(reader["user_id"]),
                Username = Convert.ToString(reader["username"]),
                PasswordHash = Convert.ToString(reader["password_hash"]),
                Salt = Convert.ToString(reader["salt"]),
                Role = Convert.ToString(reader["role_name"]),
                Team = Convert.ToString(reader["team_name"]),
                FirstName = Convert.ToString(reader["first_name"]),
                LastName = Convert.ToString(reader["last_name"]),
            };

            return u;
        }
    }
}
