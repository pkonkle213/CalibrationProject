using CalibrationApp.Models;
using System.Data.SqlClient;

namespace CalibrationApp.DAO
{
    public class SqlCalibrationDAO : ICalibrationDAO
    {
        private readonly string connectionString;

        public SqlCalibrationDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<ContactType> GetContactTypes()
        {
            List<ContactType> contactTypes = new List<ContactType>();

            const string sql = "SELECT contact_id, type " +
                               "FROM Contacts";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ContactType contact = new ContactType();

                            contact.Id = Convert.ToInt32(reader["contact_id"]);
                            contact.Name = Convert.ToString(reader["type"]); 
                            
                            contactTypes.Add(contact);
                        }
                    }
                }
            }
            return contactTypes;
        }

        public Calibration CreateCalibration(Calibration calibration)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                const string sql = "INSERT INTO Calibrations (calibration_date, leader_user_id, contact_type, contact_id, tm_first_name, tm_last_name, form_id, isOpen, group_score_earned, group_score_possible) " +
                                   "VALUES (@date, @leaderId, @type, @id, @first, @last, @form_id, @isOpen, @earned, @possible); " +
                                   "SELECT @@IDENTITY";

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@date", calibration.CalibrationDate);
                    command.Parameters.AddWithValue("@leaderId", 1); // TODO: need to implement different leaders
                    command.Parameters.AddWithValue("@type", calibration.ContactChannelId);
                    command.Parameters.AddWithValue("@id", calibration.ContactId);
                    command.Parameters.AddWithValue("@first", calibration.RepFirstName);
                    command.Parameters.AddWithValue("@last", calibration.RepLastName);
                    command.Parameters.AddWithValue("@form_id", calibration.FormId);
                    command.Parameters.AddWithValue("@isOpen", 1);
                    command.Parameters.AddWithValue("@earned", 0);
                    command.Parameters.AddWithValue("@possible", 0);

                    calibration.Id = Convert.ToInt32(command.ExecuteScalar());
                }
            }

            return calibration;
        }

        public Calibration GetCalibration(int calibrationId)
        {
            Calibration calibration = new Calibration();

            const string sql = "SELECT calibration_id, leader_user_id, tm_first_name, tm_last_name, group_score_earned, group_score_possible, contact_type, calibration_date, contact_id, form_id, isOpen " +
                               "FROM Calibrations  " +
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
                            calibration = BuildCalibration(reader);
                        }
                    }
                }
            }

            return calibration;

        }

        public List<Calibration> GetAllCalibrations()
        {
            List<Calibration> calibrations = new List<Calibration>();

            const string sql = "SELECT calibration_id, leader_user_id, tm_first_name, tm_last_name, group_score_earned, group_score_possible, contact_type, calibration_date, contact_id, form_id, isOpen " +
                               "FROM Calibrations " +
                               "ORDER BY calibration_date DESC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            calibrations.Add(BuildCalibration(reader));
                        }
                    }
                }
            }

            return calibrations;
        }

        public List<Score> GetMyScores(int userId)
        {
            List<Score> scores = new List<Score>();

            const string sql = "SELECT calibration_id, points_earned, points_possible " +
                               "FROM Scores " +
                               "WHERE user_id = @userId";

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
                            Score score = new Score();

                            score.CalibrationId = Convert.ToInt32(reader["calibration_id"]);
                            score.PointsEarned = Convert.ToDecimal(reader["points_earned"]);
                            score.PointsPossible = Convert.ToDecimal(reader["points_possible"]);

                            scores.Add(score);
                        }
                    }
                }
            }

            return scores;
        }

        private Calibration BuildCalibration(SqlDataReader reader)
        {
            Calibration calibration = new Calibration();

            calibration.Id = Convert.ToInt32(reader["calibration_id"]);
            calibration.LeaderUserId = Convert.ToInt32(reader["leader_user_id"]);
            calibration.RepFirstName = Convert.ToString(reader["tm_first_name"]);
            calibration.RepLastName = Convert.ToString(reader["tm_last_name"]);
            calibration.GroupScoreEarned = Convert.ToDecimal(reader["group_score_earned"]);
            calibration.GroupScorePossible = Convert.ToDecimal(reader["group_score_possible"]);
            calibration.ContactChannelId = Convert.ToInt32(reader["contact_type"]);
            calibration.CalibrationDate = Convert.ToDateTime(reader["calibration_date"]);
            calibration.ContactId = Convert.ToString(reader["contact_id"]);
            calibration.FormId = Convert.ToInt32(reader["form_id"]);
            calibration.IsOpen = Convert.ToBoolean(reader["isOpen"]);
          
            return calibration;
        }

        public int SwitchCalibrationIsOpen(int calibrationId)
        {
            int rowsAffected;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                const string sql = "UPDATE Calibrations " +
                                   "SET isOpen = 1 - isOpen " +
                                   "WHERE calibration_id = @calibrationId;";

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@calibrationId", calibrationId);
                    rowsAffected = command.ExecuteNonQuery();
                }
            }

            return rowsAffected;
        }
    }
}
