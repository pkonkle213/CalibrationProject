using CalibrationApp.Models;
using System.Data.SqlClient;

namespace CalibrationApp.DAO
{
    public class SqlQuestionDAO : IQuestionDAO
    {
        private readonly string connectionString;

        public SqlQuestionDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Question> GetQuestions(int calibrationId)
        {
            List<Question> questions = new List<Question>();

            const string sqlQuestion = "SELECT q.question_id,q.question,q.isCategory,q.points_possible " +
                "FROM Questions q " +
                "INNER JOIN Forms f ON f.form_id = q.form_id " +
                "INNER JOIN Calibrations c ON c.form_id = f.form_id " +
                "WHERE c.calibration_id = @calibrationId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(sqlQuestion, conn))
                {
                    command.Parameters.AddWithValue("@calibrationId", calibrationId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Question question = new Question();

                            question.Id = Convert.ToInt32(reader["question_id"]);
                            question.QuestionText = Convert.ToString(reader["question"]);
                            question.IsCategory = Convert.ToBoolean(reader["isCategory"]); // I think this should work?
                            if (question.IsCategory)
                            {
                                question.PointsPossible = Convert.ToInt32(reader["points_possible"]);
                            }
                            question.Options = GetOptions(question.Id, question.IsCategory);

                            questions.Add(question);
                        }
                    }
                }
            }

            return questions;
        }

        public List<Option> GetOptions(int questionId, bool isCategory)
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

                            option.Id = Convert.ToInt32(reader["option_id"]);
                            option.OptionValue = Convert.ToString(reader["option_value"]);
                            if (isCategory)
                            {
                                option.PointsEarned = Convert.ToDecimal(reader["points_earned"]);
                            }

                            options.Add(option);
                        }
                    }

                }
            }
            return options;
        }
    }
}
