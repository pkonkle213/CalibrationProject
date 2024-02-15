using CalibrationApp.Models;
using System.Data.SqlClient;

namespace CalibrationApp.DAO
{
    public class SqlTeamDAO : ITeamDAO
    {
        private readonly string connectionString;

        public SqlTeamDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Team> GetTeams()
        {
            List<Team> teams = new List<Team>();
            string sql = "SELECT team_id, team_name " +
                         "FROM Teams " +
                         "WHERE team_id <> 5 " +
                         "ORDER BY team_name";

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
    }
}
