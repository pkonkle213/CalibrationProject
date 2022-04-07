using CalibrationApp.Models;
using System.Data.SqlClient;

namespace CalibrationApp.DAO
{
    public class SqlAnswerDAO : IAnswerDAO
    {
        private readonly string connectionString;

        public SqlAnswerDAO(string dbConnectionString)
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

            foreach(Option option in options)
            {
                if(option.OptionValue == value)
                {
                    return option.Id;
                }
            }

            return -1;
        }

        public void SubmitScore(Score score, int userId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                const string sql = "INSERT INTO Scores (user_id,calibration_id,points_earned,points_possible) " +
                    "VALUES (@user_id,@calibration_id,@points_earned,@points_possible)";

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@user_id", userId);
                    command.Parameters.AddWithValue("@calibration_id", score.CalibrationId);
                    command.Parameters.AddWithValue("@points_earned", score.PointsEarned);
                    command.Parameters.AddWithValue("@points_possible", score.PointsPossible);

                    command.ExecuteScalar();
                }
            }
        }

        public int SubmitAnswers(List<Answer> answers, int userId)
        {
            int correct = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                const string check = "SELECT calibration_id " +
                    "FROM Calibrations " +
                    "WHERE (isOpen = 1 AND calibration_id = @calibrationId)";

                using (SqlCommand command = new SqlCommand(check, conn))
                {
                    command.Parameters.AddWithValue("@calibrationId", answers[0].CalibrationId);
                    correct = Convert.ToInt32(command.ExecuteScalar());
                }
            }

            if (correct > 0)
            {
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
                            int optionId = GetOption(answer.OptionValue);
                            command.Parameters.AddWithValue("@optionId", optionId);
                            command.Parameters.AddWithValue("@comment", answer.Comment);

                            command.ExecuteScalar();
                        }
                    }
                }
            }

            return correct;
        }

        public void UpdateScore(Score score, int userId)
        {
            int correct = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                const string check = "SELECT COUNT(calibration_id) " +
                    "FROM Calibrations " +
                    "WHERE (isOpen = 1 AND calibration_id = @calibrationId)";

                using (SqlCommand command = new SqlCommand(check, conn))
                {
                    command.Parameters.AddWithValue("@calibrationId", score.CalibrationId);
                    correct = Convert.ToInt32(command.ExecuteScalar());
                }
            }

            if (correct == 1)
            {

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    const string sql = "Update Scores " +
                        "SET points_possible = @points_possible, points_earned = @points_earned " +
                        "WHERE (user_id=@user_id AND calibration_id=@calibration_id)";

                    using (SqlCommand command = new SqlCommand(sql, conn))
                    {
                        command.Parameters.AddWithValue("@user_id", userId);
                        command.Parameters.AddWithValue("@calibration_id", score.CalibrationId);
                        command.Parameters.AddWithValue("@points_earned", score.PointsEarned);
                        command.Parameters.AddWithValue("@points_possible", score.PointsPossible);

                        command.ExecuteScalar();
                    }
                }
            }
        }

        public int UpdateAnswers(List<Answer> answers, int userId)
        {
            int correct = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                const string check = "SELECT COUNT(calibration_id) " +
                    "FROM Calibrations " +
                    "WHERE (isOpen = 1 AND calibration_id = @calibrationId)";

                using (SqlCommand command = new SqlCommand(check, conn))
                {
                    command.Parameters.AddWithValue("@calibrationId", answers[0].CalibrationId);
                    correct = Convert.ToInt32(command.ExecuteScalar());
                }
            }

            if (correct == 1)
            {
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

            return correct;
        }

        public List<User> GetParticipatingUsers(int calibrationId)
        {
            List<User> users = new List<User>();

            const string sql = "SELECT u.user_id,u.first_name,u.last_name,r.role_name,t.team_name " +
                "FROM Users u " +
                "INNER JOIN Roles r ON u.role_id = r.role_id " +
                "INNER JOIN Teams t ON u.team_id = t.team_id " +
                "WHERE (u.user_id <> 0 AND u.user_id IN " +
                "(SELECT user_id " +
                "FROM Answers " +
                "WHERE calibration_id = @calibrationId " +
                "GROUP BY user_id))";

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
                            User user = new User();

                            user.UserId = Convert.ToInt32(reader["user_id"]);
                            user.FirstName = Convert.ToString(reader["first_name"]);
                            user.LastName = Convert.ToString(reader["last_name"]);
                            user.Role = Convert.ToString(reader["role_name"]);
                            user.Team = Convert.ToString(reader["team_name"]);

                            users.Add(user);
                        }
                    }
                }
            }

            foreach(User user in users)
            {
                user.Answers = GetMyAnswers(calibrationId, user.UserId);
            }

            return users;
        }

        public List<Answer> GetMyAnswers(int calibrationId,int userId)
        {
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
    }
}
