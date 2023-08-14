using CalibrationApp.Models;
using System.Data.SqlClient;

namespace CalibrationApp.DAO
{
    public class SqlStatsDAO : IStatsDAO
    {
        private readonly string connectionString;

        private string sql = "SELECT Count(*) " +
                             "FROM Answers a " +
                             "INNER JOIN Answers b ON b.calibration_id = a.calibration_id " +
                             "INNER JOIN Calibrations c ON c.calibration_id = a.calibration_id " +
                             "WHERE (a.user_id = 0 AND b.user_id = @user_id) AND (a.calibration_id = b.calibration_id) AND (a.question_id = b.question_id)";

        private string perQuestion = " AND (a.question_id = @question_id)";

        private string perCalibration = " AND (a.calibration_id = @calibration_id)";

        private string perCalibrationType = " AND (c.contact_type = @contact_type)";

        private string modifier;

        public SqlStatsDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Question> GetAllQuestions()
        {
            List<Question> questions = new List<Question>();

            const string sql = "SELECT question_id, question " +
                "FROM Questions";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Question question = new Question();

                            question.Id = Convert.ToInt32(reader["question_id"]);
                            question.QuestionText = Convert.ToString(reader["question"]);

                            questions.Add(question);
                        }
                    }
                }
            }

            return questions;
        }

        public Calibrated GetCalibrated(int userId, string reason, int elementId)
        {
            Calibrated calibrated = new Calibrated();

            switch (reason)
            {
                case "Question":
                    sql += perQuestion;
                    modifier = "@question_id";
                    break;

                case "Calibration":
                    sql += perCalibration;
                    modifier = "@calibration_id";
                    break;

                case "Type":
                    sql += perCalibrationType;
                    modifier = "@contact_type";
                    break;

                default:
                    break;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@user_id", userId);
                    
                    if (elementId != 0)
                    {
                        command.Parameters.AddWithValue(modifier, elementId);
                    }

                    calibrated.Possible = Convert.ToInt32(command.ExecuteScalar());
                }
            }

            string sqlCorrect = sql + " AND (a.option_id = b.option_id)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(sqlCorrect, conn))
                {
                    command.Parameters.AddWithValue("@user_id", userId);

                    if (elementId != 0)
                    {
                        command.Parameters.AddWithValue(modifier, elementId);
                    }

                    calibrated.Correct = Convert.ToInt32(command.ExecuteScalar());
                }
            }

            return calibrated;
        }
    }
}
