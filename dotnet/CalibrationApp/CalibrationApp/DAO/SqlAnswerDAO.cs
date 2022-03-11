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

        public Answer SubmitAnswer(Answer answer)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                const string sql = "INSERT INTO Answers (calibration_id,user_id,question_id,option_id,comment) " +
                    "VALUES(@calibrationId, @userId, @questionId, @optionId, @comment)";

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@calibrationId", answer.CalibrationId);
                    command.Parameters.AddWithValue("@userId", answer.UserId);
                    command.Parameters.AddWithValue("@questionId", answer.QuestionId);
                    command.Parameters.AddWithValue("@optionId", answer.OptionId);
                    command.Parameters.AddWithValue("@comment", answer.Comment);

                    command.ExecuteScalar();
                }
            }

            return answer;
        }

        public List<Answer> GetAnswersForCalibration(int calibrationId)
        {
            List<Answer> answers = new List<Answer>();

            const string sql = "SELECT user_id,question_id,option_id,comment " +
                "FROM Answers " +
                "WHERE calibration_id = @calibrationId";

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
                            Answer answer = new Answer();

                            answer.CalibrationId = calibrationId;
                            answer.UserId = Convert.ToInt32(reader["@user_id"]);
                            answer.UserId = Convert.ToInt32(reader["@question_id"]);
                            answer.UserId = Convert.ToInt32(reader["@option_id"]);
                            answer.UserId = Convert.ToInt32(reader["@comment"]);

                            answers.Add(answer);
                        }
                    }
                }
            }

            return answers;
        }

        public List<User> GetParticipatingUsers(int calibrationId)
        {
            List<User> users = new List<User>();

            const string sql = "SELECT u.user_id,u.first_name,u.last_name,r.role_name,t.team_name " +
                "FROM Users u " +
                "INNER JOIN Roles r ON u.role_id = r.role_id " +
                "INNER JOIN Teams t ON u.team_id = t.team_id " +
                "WHERE u.user_id IN " +
                "(SELECT user_id " +
                "FROM Answers " +
                "WHERE calibration_id = @calibrationId " +
                "GROUP BY user_id)";

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

            return users;
        }
    }
}
