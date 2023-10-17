using CalibrationApp.Models;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

namespace CalibrationApp.DAO
{
    public class SqlQuestionDAO : IQuestionDAO
    {
        private readonly string connectionString;
        private readonly IOptionDAO optionDAO;

        public SqlQuestionDAO(string dbConnectionString, IOptionDAO optionDAO)
        {
            connectionString = dbConnectionString;
            optionDAO = this.optionDAO;
        }

        public List<Question> GetEditQuestionsByForm(int formId)
        {
            List<Question> questions = GetQuestionsByFormId(formId);
            List<Option> options = optionDAO.GetAllOptions(formId);

            List<Option> categoryOptions = options.FindAll(x => x.IsCategory);
            List<Option> nonCategoryOptions = options.FindAll(x => !x.IsCategory);

            foreach (Question question in questions)
            {
                List<Option> checkOptions = question.IsCategory ? categoryOptions : nonCategoryOptions;

                foreach (Option option in checkOptions)
                {
                    var optionExists = question.Options.Exists(x => x.Id == option.Id);
                    if (!optionExists)
                    {
                        option.IsEnabled = false;
                        question.Options.Add(option);
                    }
                    else
                    {
                        var currentQuestion = questions.Find(x => x.Id ==  question.Id);
                        var currentOption = currentQuestion.Options.Find(x => x.Id == option.Id);
                        currentOption.IsEnabled = true;
                    }
                }

                question.Options = question.Options.OrderBy(x => x.OrderPosition).ToList();
            }

            return questions;
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
            question.Options = optionDAO.GetEnabledOptionsForQuestion(question.Id);

            return question;
        }

        public Question UpdateQuestion(Question question)
        {
            // Does this use of a transaction work?!

            const string sql = "UPDATE Questions " +
                               "SET form_id = @form_id, form_position = @form_position, question = @question, isCategory = @isCategory, points_possible = @points_possible " +
                               "WHERE question_Id = @questionId";

            int rowsAffected;
            var trans = new TransactionScope();

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

                if (rowsAffected != 1)
                {
                    trans?.Dispose();
                    throw new Exception("There was an issue with the SQL");
                }
                else
                {
                    trans.Complete();
                }
            }

            return question;
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
    }
}
