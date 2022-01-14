using CalibrationApp.Models;
using System.Data.SqlClient;

namespace CalibrationApp.DAO
{
    public class SQLQuestionDAO : IQuestionDAO
    {
        private readonly string connectionString;

        public SQLQuestionDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Question> GetQuestions(int calibrationId)
        {
            List<Question> questions = new List<Question>();

            const string sql = "SELECT q.question_id,q.question,q.isCategory,q.points_possible " +
                "FROM Questions q " +
                "INNER JOIN Forms f ON f.form_id = q.form_id " +
                "INNER JOIN Calibrations c ON c.form_id = f.form_id " +
                "WHERE c.calibration_id = @calibrationId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@calibrationId", calibrationId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Question question = new Question();

                            question.Id = Convert.ToInt32(reader["q.question_id"]);
                            question.QuestionText = Convert.ToString(reader["q.question"]);
                            question.IsCategory = Convert.ToBoolean(reader["q.isCategory"]); // I think this should work?
                            question.PointsPossible = Convert.ToInt32(reader["q.points_possible"]);
                            question.Options = GetOptions(question.Id);

                            questions.Add(question);
                        }
                    }
                }
            }

            return questions;
        }

        public List<Option> GetOptions(int questionId)
        {
            List<Option> options = new List<Option>();

            const string sql = "SELECT o.option_id,o.option_value,o.points_earned " +
                "FROM Options o " +
                "INNER JOIN Questions_Options qo ON qo.option_id = o.option_id " +
                "WHERE qo.question_id = @questionId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@questionId", questionId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Option option = new Option();

                            option.Id = Convert.ToInt32(reader["o.option_id"]);
                            option.OptionValue= Convert.ToString(reader["o.option_value"]);
                            option.PointsEarned = Convert.ToDecimal(reader["o.points_earned"]);

                            options.Add(option);
                        }
                    }

                }
            }
            return options;
        }
    }
}