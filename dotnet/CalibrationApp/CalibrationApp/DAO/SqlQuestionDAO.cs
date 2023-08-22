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

        public List<Question> GetQuestionsByCalibrationId(int calibrationId)
        {
            List<Question> questions = new List<Question>();

            const string sql = "SELECT q.question_id, q.form_id, q.form_position, q.question, q.isCategory, q.points_possible " +
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
                            questions.Add(BuildQuestionFromReader(reader));
                        }
                    }
                }
            }

            return questions;
        }

        private Question BuildQuestionFromReader(SqlDataReader reader)
        {
            Question question = new Question();

            question.Id = Convert.ToInt32(reader["question_id"]);
            question.FormId = Convert.ToInt32(reader["form_id"]);
            question.FormPosition = Convert.ToInt32(reader["form_position"]);
            question.QuestionText = Convert.ToString(reader["question"]);
            question.PointsPossible = Convert.ToInt32(reader["points_possible"]);
            //question.Options = GetOptions(question.IsCategory);

            return question;
        }

        public int UpdateQuestion(Question question)
        {
            const string sql = "UPDATE Questions " +
                               "SET form_id = @form_id, form_position = @form_position, question = @question, isCategory = @isCategory, points_possible = @points_possible " +
                               "WHERE question_Id = @questionId; " +
                               "SELECT @@IDENTITY";

            int rowsAffected;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@questionId", question.Id);
                    command.Parameters.AddWithValue("@form_id", question.FormId);
                    command.Parameters.AddWithValue("@form_position", question.FormPosition);
                    command.Parameters.AddWithValue("@question", question.QuestionText);
                    command.Parameters.AddWithValue("@isCategory", question.IsCategory);
                    command.Parameters.AddWithValue("@points_possible", question.PointsPossible);

                    rowsAffected = command.ExecuteNonQuery();
                }
            }

            return rowsAffected;
        }
        public Question NewQuestion(Question question)
        {
            const string sql = "INSERT INTO Questions (form_id, form_position, question, isCategory, points_possible) " +
                                "VALUES (@form_id, @form_position, @question, @isCategory, @points_possible); " +
                                "SELECT @@IDENTITY";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@form_id", question.FormId);
                    command.Parameters.AddWithValue("@form_position", question.FormPosition);
                    command.Parameters.AddWithValue("@question", question.QuestionText);
                    command.Parameters.AddWithValue("@isCategory", question.IsCategory);
                    command.Parameters.AddWithValue("@points_possible", question.PointsPossible);

                    question.Id = Convert.ToInt32(command.ExecuteScalar());
                }
            }

            return question;
        }

        public List<Question> GetQuestionsByFormId(int formId)
        {
            List<Question> questions = new List<Question>();

            const string sql = "SELECT question_id, form_id, form_position, question, isCategory, points_possible " +
                               "FROM Questions " +
                               "WHERE form_id = @formId " +
                               "ORDER BY form_position";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@formId", formId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            questions.Add(BuildQuestionFromReader(reader));
                        }
                    }
                }
            }

            return questions;
        }

        //public List<Option> GetAllOptions()
        //{
        //    List<Option> options = new List<Option>();

        //    const string sql = "SELECT option_id, isCategory, option_value, points_earned " +
        //                       "FROM Options";

        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        conn.Open();

        //        using (SqlCommand command = new SqlCommand(sql, conn))
        //        {
        //            using (SqlDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    Option option = new Option();

        //                    option.Id = Convert.ToInt32(reader["option_id"]);
        //                    option.isCategory = Convert.ToBoolean(reader["isCategory"]);
        //                    option.OptionValue = Convert.ToString(reader["option_value"]);
        //                    option.PointsEarned = Convert.ToDecimal(reader["points_earned"]);

        //                    options.Add(option);
        //                }
        //            }

        //        }
        //    }

        //    return options;
        //}

        public List<Option> GetOptions(bool isCategory)
        {
            List<Option> options = new List<Option>();

            const string sql = "SELECT option_id, isCategory, option_value, points_earned " +
                               "FROM Options " +
                               "WHERE isCategory = @isCategory";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@isCategory", isCategory);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Option option = new Option();

                            option.Id = Convert.ToInt32(reader["option_id"]);
                            option.OptionValue = Convert.ToString(reader["option_value"]);
                            option.PointsEarned = Convert.ToDecimal(reader["points_earned"]);

                            options.Add(option);
                        }
                    }

                }
            }

            return options;
        }
    }
}
