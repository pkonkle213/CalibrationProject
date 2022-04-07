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

        private int GetTypeId(string name)
        {
            List<ContactType> contactTypes = GetContactTypes();

            foreach(ContactType contactType in contactTypes)
            {
                if(contactType.Name == name)
                {
                    return contactType.Id;
                }
            }

            return -1;
        }

        public List<ContactType> GetContactTypes()
        {
            List<ContactType> contactTypes = new List<ContactType>();

            const string sql = "SELECT contact_id,type " +
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
                            contactTypes.Add(BuildContactType(reader));
                        }
                    }
                }
            }
            return contactTypes;
        }

        private ContactType BuildContactType(SqlDataReader reader)
        {
            ContactType contact = new ContactType();

            contact.Id = Convert.ToInt32(reader["contact_id"]);
            contact.Name = Convert.ToString(reader["type"]);

            return contact;
        }

        public Calibration CreateCalibration(Calibration calibration)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                const string sql = "INSERT INTO Calibrations (calibration_date,contact_type,contact_id,tm_first_name,tm_last_name,form_id,isOpen,group_score_earned,group_score_possible) " +
                    "VALUES (@date,@type,@id,@first,@last,@form_id,@isOpen,@earned,@possible); " +
                    "SELECT @@IDENTITY";

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@date", calibration.CalibrationDate);
                    command.Parameters.AddWithValue("@type", GetTypeId(calibration.ContactChannel));
                    command.Parameters.AddWithValue("@id", calibration.ContactId);
                    command.Parameters.AddWithValue("@first", calibration.RepFirstName);
                    command.Parameters.AddWithValue("@last", calibration.RepLastName);
                    command.Parameters.AddWithValue("@form_id", 1);
                    command.Parameters.AddWithValue("@isOpen", 1);
                    command.Parameters.AddWithValue("@earned", 0);
                    command.Parameters.AddWithValue("@possible", 0);
                    calibration.Id = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            return calibration;
        }

        public Calibration GetCalibration(int calibrationId, int userId)
        {
            Calibration calibration = new Calibration();

            const string sql = "SELECT c.calibration_id,c.tm_first_name,c.tm_last_name,c.group_score_earned,c.group_score_possible,con.type,c.calibration_date,c.contact_id,c.isOpen " +
                "FROM Calibrations c " +
                "INNER JOIN Contacts con ON con.contact_id = c.contact_type " +
                "WHERE c.calibration_id=@calibrationId";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@calibrationId",calibrationId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            calibration=BuildCalibration(reader);
                        }
                    }
                }
            }

            return calibration;

        }

        public List<Calibration> GetAllCalibrations(int userId)
        {
            List<Calibration> calibrations = new List<Calibration>();

            const string sql = "SELECT c.calibration_id,c.tm_first_name,c.tm_last_name,c.group_score_earned,c.group_score_possible,con.type,c.calibration_date,c.contact_id,c.isOpen " +
                "FROM Calibrations c " +
                "INNER JOIN Contacts con ON con.contact_id = c.contact_type " +
                "ORDER BY c.calibration_date DESC";

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

            const string sql = "SELECT calibration_id,points_earned,points_possible " +
                "FROM Scores " +
                "WHERE user_id=@userId";

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
            calibration.RepFirstName = Convert.ToString(reader["tm_first_name"]);
            calibration.RepLastName = Convert.ToString(reader["tm_last_name"]);
            calibration.GroupScoreEarned = Convert.ToDecimal(reader["group_score_earned"]);
            calibration.GroupScorePossible = Convert.ToDecimal(reader["group_score_possible"]);
            calibration.ContactChannel = Convert.ToString(reader["type"]);
            calibration.CalibrationDate = Convert.ToDateTime(reader["calibration_date"]);
            calibration.ContactId = Convert.ToString(reader["contact_id"]);
            calibration.IsOpen = Convert.ToBoolean(reader["isOpen"]);
            return calibration;
        }

        public void SwitchCalibrationIsOpen(int calibrationId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                const string sql = "UPDATE Calibrations " +
                    "SET isOpen = 1 - isOpen " +
                    "WHERE calibration_id = @calibrationId;";

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@calibrationId", calibrationId);
                    command.ExecuteScalar();
                }
            }
        }

        public void SwitchCalibrationIsActive(int calibrationId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                const string sql = "UPDATE Calibrations " +
                    "SET isOpen = 1 - isOpen " +
                    "WHERE calibration_id = @calibrationId;";

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@calibrationId", calibrationId);
                    command.ExecuteScalar();
                }
            }
        }

        public Calibration GetCalibrationResults(int calibrationId)
        {
            Calibration calibration = new Calibration();
            return calibration;
        }
    }
}
