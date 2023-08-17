using CalibrationApp.Models;
using System.Data.SqlClient;

namespace CalibrationApp.DAO
{
    public class SqlParticipantDAO : IParticipantDAO
    {
        private readonly string connectionString;
        public SqlParticipantDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Team> GetTeams()
        {
            List<Team> teams = new List<Team>();

            const string sql = "SELECT team_id, team_name " +
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

        public List<Role> GetRoles()
        {
            List<Role> roles = new List<Role>();

            const string sql = "SELECT role_id, role_name " +
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

        public int SwitchActive(int userId)
        {
            const string sql =  "UPDATE Users " +
                                "SET isActive = 1 - isActive " +
                                "WHERE user_id = @userId";

            int rowsAffected;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    rowsAffected = command.ExecuteNonQuery();
                }
            }

             return rowsAffected;
        }

        public List<Participant> GetAllParticipants()
        {
            List<Participant> participants = new List<Participant>();

            const string sql = "SELECT user_id, first_name, last_name " +
                               "FROM Users";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Participant participant = new Participant();

                            participant.UserId = Convert.ToInt32(reader["user_id"]);
                            participant.FirstName = Convert.ToString(reader["first_name"]);
                            participant.LastName = Convert.ToString(reader["last_name"]);

                            participants.Add(participant);
                        }
                    }
                }
            }

            return participants;
        }
    }
}
