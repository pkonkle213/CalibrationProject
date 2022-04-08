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

        public List<Answer> GetMyAnswers(int userId)
        {
            return GetAnswers(userId);
        }

        public List<Answer> GetGroupAnswers()
        {
            return GetAnswers(0);
        }

        private List<Answer> GetAnswers(int userId)
        {
            List<Answer> answers = new List<Answer>();

            const string sql = "SELECT a.calibration_id,a.user_id,a.question_id,o.option_value,a.comment,o.points_earned " +
                "FROM Answers a " +
                "INNER JOIN Options o ON o.option_id = a.option_id " +
                "WHERE a.user_id = @userId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@userId", userId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Answer answer = new Answer();

                            answer.CalibrationId = Convert.ToInt32(reader["calibration_id"]);
                            answer.OptionValue = Convert.ToString(reader["option_value"]);
                            answer.QuestionId = Convert.ToInt32(reader["question_id"]);
                            answer.Comment = Convert.ToString(reader["comment"]);
                            answer.PointsEarned = Convert.ToDecimal(reader["points_earned"]);

                            answers.Add(answer);
                        }
                    }
                }
            }

            return answers;
        }
    }
}
