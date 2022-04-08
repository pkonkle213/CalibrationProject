using CalibrationApp.Models;
using System.Data.SqlClient;

namespace CalibrationApp.DAO
{
    public class SqlGroupDAO : IGroupDAO
    {
        private readonly string connectionString;

        public SqlGroupDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        private int GetOption(string value)
        {
            List<Option> options = new List<Option>();

            const string sql = "SELECT option_id,option_value " +
                "FROM Options";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Option option = new Option();

                            option.Id = Convert.ToInt32(reader["option_id"]);
                            option.OptionValue = Convert.ToString(reader["option_value"]);

                            options.Add(option);
                        }
                    }
                }
            }

            foreach (Option option in options)
            {
                if (option.OptionValue == value)
                {
                    return option.Id;
                }
            }

            return -1;
        }

        public List<Answer> GetGroupAnswers(int calibrationId)
        {
            int userId = 0;
            List<Answer> answers = new List<Answer>();

            const string sql = "SELECT a.calibration_id,a.user_id,a.question_id,o.option_value,a.comment,o.points_earned " +
                "FROM Answers a " +
                "INNER JOIN Options o ON o.option_id = a.option_id " +
                "WHERE (a.calibration_id = @calibrationId AND a.user_id = @userId)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@calibrationId", calibrationId);
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

        public void UpdateScore(Score score)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                const string sql = "Update Calibrations " +
                    "SET group_score_earned = @points_earned, group_score_possible = @points_possible " +
                    "WHERE calibration_id = @calibration_id";

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@calibration_id", score.CalibrationId);
                    command.Parameters.AddWithValue("@points_earned", score.PointsEarned);
                    command.Parameters.AddWithValue("@points_possible", score.PointsPossible);

                    command.ExecuteScalar();
                }
            }
        }

        public void SubmitAnswers(List<Answer> answers)
        {
            int userId = 0;

            foreach (Answer answer in answers)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    const string sql = "INSERT INTO Answers (calibration_id,user_id,question_id,option_id,comment) " +
                        "VALUES(@calibrationId, @userId, @questionId, @optionId, @comment)";

                    using (SqlCommand command = new SqlCommand(sql, conn))
                    {
                        command.Parameters.AddWithValue("@calibrationId", answer.CalibrationId);
                        command.Parameters.AddWithValue("@userId", userId);
                        command.Parameters.AddWithValue("@questionId", answer.QuestionId);
                        command.Parameters.AddWithValue("@optionId", GetOption(answer.OptionValue));
                        command.Parameters.AddWithValue("@comment", answer.Comment);

                        command.ExecuteScalar();
                    }
                }
            }
        }

        public void UpdateAnswers(List<Answer> answers)
        {
            int userId = 0;

            foreach (Answer answer in answers)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    const string sql = "UPDATE Answers " +
                        "SET option_id = @option_id, comment = @comment " +
                        "WHERE (calibration_id = @calibration_id AND user_id = @user_id AND question_id = @question_id)";

                    using (SqlCommand command = new SqlCommand(sql, conn))
                    {
                        command.Parameters.AddWithValue("@calibration_id", answer.CalibrationId);
                        command.Parameters.AddWithValue("@user_id", userId);
                        command.Parameters.AddWithValue("@question_id", answer.QuestionId);
                        command.Parameters.AddWithValue("@option_id", GetOption(answer.OptionValue));
                        command.Parameters.AddWithValue("@comment", answer.Comment);

                        command.ExecuteScalar();
                    }
                }
            }
        }
    }
}
