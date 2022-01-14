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

        public void SubmitAnswer(Answer answer)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                const string sql = "INSERT INTO Answers (calibration_id,user_id,question_id,option_id,comment) " +
                    "VALUES(@calibrationId, @userId, @questionId, @optionId, @comment)";

                using (SqlCommand command = conn.CreateCommand())
                {
                    command.Parameters.AddWithValue("@calibrationId", answer.CalibrationId);
                    command.Parameters.AddWithValue("@userId", answer.UserId);
                    command.Parameters.AddWithValue("@questionId", answer.QuestionId);
                    command.Parameters.AddWithValue("@optionId", answer.OptionId);
                    command.Parameters.AddWithValue("@comment", answer.Comment);

                    command.ExecuteScalar();
                }
            }
        }

        public List<Answer> GetAnswersFromUser(int calibrationId, int userId)
        {
            List<Answer> answers = new List<Answer>();

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
                "WHERE calibration_id = 1 " +
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

                            user.UserId = Convert.ToInt32(reader["u.user_id"]);
                            user.FirstName = Convert.ToString(reader["u.first_name"]);
                            user.LastName = Convert.ToString(reader["u.last_name"]);
                            user.Role = Convert.ToString(reader["r.role_name"]);
                            user.Team = Convert.ToString(reader["t.team_name"]);

                            users.Add(user);
                        }
                    }
                }
            }

            return users;
        }
    }
}
