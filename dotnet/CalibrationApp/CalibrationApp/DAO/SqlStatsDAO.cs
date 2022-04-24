using CalibrationApp.Models;
using System.Data.SqlClient;

namespace CalibrationApp.DAO
{
    public class SqlStatsDAO : IStatsDAO
    {
        private readonly string connectionString;

        public SqlStatsDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Question> GetAllQuestions()
        {
            List<Question> questions = new List<Question>();

            const string sql = "SELECT question_id,question " +
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

        public Calibrated GetCalibrated(int userId,int questionId)
        {
            Calibrated calibrated = new Calibrated();

            string sqlPossible = "SELECT Count(*) " +
                "FROM Answers a, Answers b " +
                "WHERE (a.user_id=0 AND b.user_id=@user_id) AND (a.calibration_id = b.calibration_id) AND (a.question_id = b.question_id)";

            if (questionId!=0)
            {
                sqlPossible += " AND (a.question_id = @question_id)";
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand (sqlPossible, conn))
                {
                    command.Parameters.AddWithValue("@user_id", userId);
                    if (questionId != 0)
                    {
                        command.Parameters.AddWithValue("@question_id", questionId);
                    }
                    
                    calibrated.Possible = Convert.ToInt32(command.ExecuteScalar());
                }
            }

            sqlPossible += " AND (a.option_id = b.option_id)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(sqlPossible, conn))
                {
                    command.Parameters.AddWithValue("@user_id", userId);
                    if (questionId != 0)
                    {
                        command.Parameters.AddWithValue("@question_id", questionId);
                    }

                    calibrated.Correct = Convert.ToInt32(command.ExecuteScalar());
                }
            }

            return calibrated;
        }
    }
}
