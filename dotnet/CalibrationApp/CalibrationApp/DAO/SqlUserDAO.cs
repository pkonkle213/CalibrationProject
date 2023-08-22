using System;
using System.Data.SqlClient;
using CalibrationApp.Models;
using CalibrationApp.Security;
using CalibrationApp.Security.Models;

namespace CalibrationApp.DAO
{
    public class SqlUserDAO : IUserDAO
    {
        private string sqlGetUser = "SELECT user_id, username, password_hash, salt, role, isActive, team_id, first_name, last_name " +
                                    "FROM Users " +
                                    "WHERE username = @username";

        private string sqlAddUser = "INSERT INTO Users (username, password_hash, salt, first_name, last_name, isActive, role, team_id) VALUES " +
                                    "(@username, @password_hash, @salt, @first_name, @last_name, @isActive, @role, @team_id); " +
                                    "SELECT @@IDENTITY";

        private readonly string connectionString;
        public SqlUserDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public SaltedUser GetUser(string username)
        {
            SaltedUser returnUser = null;

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
                         "FROM Teams " +
                         "WHERE team_id <> 5";

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

        public List<StandardUser> GetAllUsers()
        {
            List<StandardUser> users = new List<StandardUser>();
            string sql = "SELECT user_id, username, role, isActive, team_id, first_name, last_name " +
                "FROM Users " +
                "WHERE user_id <> 0 " +
                "ORDER BY isActive DESC, team_id, last_name";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new StandardUser()
                            {
                                UserId = Convert.ToInt32(reader["user_id"]),
                                Username = Convert.ToString(reader["username"]),
                                Role = Convert.ToString(reader["role"]),
                                TeamId = Convert.ToInt32(reader["team_id"]),
                                FirstName = Convert.ToString(reader["first_name"]),
                                LastName = Convert.ToString(reader["last_name"]),
                                IsActive = Convert.ToBoolean(reader["isActive"]),
                            });
                        }
                    }
                }
            }

            return users;
        }

        public StandardUser AddUser(RegisterUser regUser)
        {
            IPasswordHasher passwordHasher = new PasswordHasher();
            PasswordHash hash = passwordHasher.ComputeHash(regUser.Password);
            int newUserId;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sqlAddUser, conn);
                cmd.Parameters.AddWithValue("@username", regUser.Username);
                cmd.Parameters.AddWithValue("@password_hash", hash.Password);
                cmd.Parameters.AddWithValue("@salt", hash.Salt);
                cmd.Parameters.AddWithValue("@first_name", regUser.FirstName);
                cmd.Parameters.AddWithValue("@last_name", regUser.LastName);
                cmd.Parameters.AddWithValue("@isActive", true);
                cmd.Parameters.AddWithValue("@role", regUser.Role);
                cmd.Parameters.AddWithValue("@team_id", regUser.TeamId);
                newUserId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            StandardUser newUser = new StandardUser()
            {
                UserId = newUserId,
                Username = regUser.Username,
                FirstName = regUser.FirstName,
                LastName = regUser.LastName,
                Role = regUser.Role,
                TeamId = regUser.TeamId,
                IsActive = true,
            };

            return newUser;
        }

        public int UpdateUser(StandardUser user)
        {
            int rowsAffected; using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                const string sql = "UPDATE Users " +
                    "SET username = @username, first_name = @firstName, last_name = @lastName, role = @role, team_id = @teamId " +
                    "WHERE user_id = @user_id";

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@user_id", user.UserId);
                    command.Parameters.AddWithValue("@username", user.Username);
                    command.Parameters.AddWithValue("@firstName", user.FirstName);
                    command.Parameters.AddWithValue("@lastName", user.LastName);
                    command.Parameters.AddWithValue("@role", user.Role);
                    command.Parameters.AddWithValue("@teamId", user.TeamId);
                    rowsAffected = command.ExecuteNonQuery();
                }
            }

            return rowsAffected;
        }

        public int SwitchActive(int userId)
        {
            int rowsAffected;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                const string sql = "UPDATE Users " +
                    "SET isActive = 1 - isActive " +
                    "WHERE user_id = @user_id";

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@user_id", userId);
                    rowsAffected = command.ExecuteNonQuery();
                }
            }

            return rowsAffected;
        }

        private SaltedUser GetUserFromReader(SqlDataReader reader)
        {
            SaltedUser u = new SaltedUser()
            {
                UserId = Convert.ToInt32(reader["user_id"]),
                Username = Convert.ToString(reader["username"]),
                PasswordHash = Convert.ToString(reader["password_hash"]),
                Salt = Convert.ToString(reader["salt"]),
                Role = Convert.ToString(reader["role"]),
                TeamId = Convert.ToInt32(reader["team_id"]),
                FirstName = Convert.ToString(reader["first_name"]),
                LastName = Convert.ToString(reader["last_name"]),
                IsActive = Convert.ToBoolean(reader["isActive"]),
            };

            return u;
        }
    }
}
