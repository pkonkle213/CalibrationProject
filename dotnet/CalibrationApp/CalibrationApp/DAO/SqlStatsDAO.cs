using CalibrationApp.Models;
using System.Data.SqlClient;

namespace CalibrationApp.DAO
{
    public class SqlStatsDAO : IStatsDAO
    {
        private readonly string connectionString;

        private string _correct;
        private string _possible;

        public SqlStatsDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public Statistic GetGeneralCalibrated(int userId)
        {
            _correct =  "SELECT COUNT(*) AS Correct " +
                        "FROM Answers a " +
                        "INNER JOIN Answers b ON b.calibration_id = a.calibration_id " +
                        "WHERE (a.user_id = 0 AND b.user_id = @user_id) " +
                        "AND (a.question_id = b.question_id) " +
                        "AND (a.option_id = b.option_id)";

            _possible = "SELECT COUNT(*) AS Possible " +
                        "FROM Answers a " +
                        "INNER JOIN Answers b ON b.calibration_id = a.calibration_id " +
                        "WHERE (a.user_id = 0 AND b.user_id = @user_id) " +
                        "AND (a.question_id = b.question_id)";

            Statistic stat = new Statistic();

            stat.ElementId = 0;
            stat.Description = "None";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(_possible, conn))
                {
                    command.Parameters.AddWithValue("@user_id", userId);

                    stat.Possible = Convert.ToInt32(command.ExecuteScalar());
                }

                using (SqlCommand command = new SqlCommand(_correct, conn))
                {
                    command.Parameters.AddWithValue("@user_id", userId);

                    stat.Correct = Convert.ToInt32(command.ExecuteScalar());
                }
            }

            return stat;
        }

        public List<Statistic> GetCalibrated(int userId, string reason)
        {
            switch (reason)
            {
                case "Calibration":
                    _correct = "SELECT a.calibration_id AS ElementId, c.calibration_date AS Description, COUNT(*) AS Correct " +
                                "FROM Answers a " +
                                "INNER JOIN Answers b ON b.calibration_id = a.calibration_id " +
                                "INNER JOIN Calibrations c ON a.calibration_id = c.calibration_id " +
                                "WHERE (a.user_id = 0 AND b.user_id = @user_id) " +
                                "AND (a.question_id = b.question_id) " +
                                "AND (a.option_id = b.option_id) " +
                                "GROUP BY a.calibration_id, c.calibration_date";

                    _possible = "SELECT a.calibration_id AS ElementId, c.calibration_date AS Description, COUNT(*) AS Possible " +
                                "FROM Answers a " +
                                "INNER JOIN Answers b ON b.calibration_id = a.calibration_id " +
                                "INNER JOIN Calibrations c ON a.calibration_id = c.calibration_id " +
                                "WHERE (a.user_id = 0 AND b.user_id = @user_id) " +
                                "AND (a.question_id = b.question_id) " +
                                "GROUP BY a.calibration_id, c.calibration_date";
                    break;

                case "Question":
                    _correct = "SELECT a.question_id AS ElementId, q.question AS Description, COUNT(*) AS Correct " +
                                "FROM Answers a " +
                                "INNER JOIN Answers b ON b.calibration_id = a.calibration_id " +
                                "INNER JOIN Questions q ON a.question_id = q.question_id " +
                                "WHERE (a.user_id = 0 AND b.user_id = @user_id) " +
                                "AND (a.question_id = b.question_id) " +
                                "AND (a.option_id = b.option_id) " +
                                "GROUP BY a.question_id, q.question";

                    _possible = "SELECT a.question_id AS ElementId, q.question AS Description, COUNT(*) AS Possible " +
                                "FROM Answers a " +
                                "INNER JOIN Answers b ON b.calibration_id = a.calibration_id " +
                                "INNER JOIN Questions q ON a.question_id = q.question_id " +
                                "WHERE (a.user_id = 0 AND b.user_id = @user_id) " +
                                "AND (a.question_id = b.question_id) " +
                                "GROUP BY a.question_id, q.question";
                    break;

                case "Type":
                    _correct = "SELECT c.contact_type AS ElementId, cs.type AS Description, COUNT(*) AS Correct " +
                                "FROM Answers a " +
                                "INNER JOIN Answers b ON b.calibration_id = a.calibration_id " +
                                "INNER JOIN Calibrations c ON a.calibration_id = c.calibration_id " +
                                "INNER JOIN ContactTypes cs ON cs.contact_id = c.contact_type " +
                                "WHERE (a.user_id = 0 AND b.user_id = @user_id) " +
                                "AND (a.question_id = b.question_id) " +
                                "AND (a.option_id = b.option_id) " +
                                "GROUP BY c.contact_type, cs.type";

                    _possible = "SELECT c.contact_type AS ElementId, cs.type AS Description, COUNT(*) AS Possible " +
                                "FROM Answers a " +
                                "INNER JOIN Answers b ON b.calibration_id = a.calibration_id " +
                                "INNER JOIN Calibrations c ON a.calibration_id = c.calibration_id " +
                                "INNER JOIN ContactTypes cs ON cs.contact_id = c.contact_type " +
                                "WHERE (a.user_id = 0 AND b.user_id = @user_id) " +
                                "AND (a.question_id = b.question_id) " +
                                "GROUP BY c.contact_type, cs.type";
                    break;

                default:
                    break;
            }

            List<Statistic> statistics = new List<Statistic>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(_possible, conn))
                {
                    command.Parameters.AddWithValue("@user_id", userId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Statistic stat = new Statistic();

                            stat.ElementId = Convert.ToInt32(reader["ElementId"]);
                            stat.Description = Convert.ToString(reader["Description"]);
                            stat.Possible = Convert.ToInt32(reader["Possible"]);

                            statistics.Add(stat);
                        }
                    }
                }

                using (SqlCommand command = new SqlCommand(_correct, conn))
                {
                    command.Parameters.AddWithValue("@user_id", userId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Statistic stat = new Statistic();

                            stat.ElementId = Convert.ToInt32(reader["ElementId"]);
                            stat.Description = Convert.ToString(reader["Description"]);
                            stat.Correct = Convert.ToInt32(reader["Correct"]);

                            var possibleStat = statistics.FirstOrDefault(x => x.ElementId == stat.ElementId);

                            if (possibleStat == null)
                            {
                                throw new Exception("Couldn't find the matching stat");
                            }

                            statistics.FirstOrDefault(x => x.ElementId == stat.ElementId).Correct = stat.Correct;
                        }
                    }
                }
            }

            return statistics;
        }
    }
}
