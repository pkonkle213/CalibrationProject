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

        private List<Contact> GetAllContactTypes()
        {
            List <Contact> contacts = new List<Contact>();
            return contacts;
        }


        //public Calibration CreateCalibration(DateTime date, int type, string contactId, string first, string last)
        //{
        //    Calibration calibration = new Calibration();
        //    calibration.CalibrationDate = date;
        //    calibration.TypeId = type;
        //    calibration.ContactId = contactId;
        //    calibration.RepFirstName = first;
        //    calibration.RepLastName = last;

        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        conn.Open();

        //        const string sql = "INSERT INTO Calibrations (calibration_date,contact_type,contact_id,tm_first_name,tm_last_name,form_id,isOpen) " +
        //            "VALUES (@date,@type,@id,@first,@last,@form_id,@isOpen); " +
        //            "SELECT @@IDENTITY";

        //        using (SqlCommand command = new SqlCommand(sql, conn))
        //        {
        //            command.Parameters.AddWithValue("@date", calibration.CalibrationDate);
        //            command.Parameters.AddWithValue("@type", calibration.TypeId);
        //            command.Parameters.AddWithValue("@id", calibration.ContactId);
        //            command.Parameters.AddWithValue("@first", calibration.RepFirstName);
        //            command.Parameters.AddWithValue("@last", calibration.RepLastName);
        //            command.Parameters.AddWithValue("@form_id", 1);
        //            command.Parameters.AddWithValue("@isOpen", 1);
        //            calibration.Id = Convert.ToInt32(command.ExecuteScalar());
        //        }
        //    }
        //    return calibration;
        //}

        public Calibration GetCalibration(int calibrationId)
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

        public List<Calibration> GetAllCalibrations()
        {
            List<Calibration> calibrations = new List<Calibration>();

            const string sql = "SELECT c.calibration_id,c.tm_first_name,c.tm_last_name,c.group_score_earned,c.group_score_possible,con.type,c.calibration_date,c.contact_id,c.isOpen " +
                "FROM Calibrations c " +
                "INNER JOIN Contacts con ON con.contact_id = c.contact_type " +
                "ORDER BY c.calibration_date DESC ";

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

        private static Calibration BuildCalibration(SqlDataReader reader)
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
            calibration.IsActive = Convert.ToBoolean(reader["isOpen"]);
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

        public Calibration GetCalibrationResults(int calibrationId)
        {
            Calibration calibration = new Calibration();
            return calibration;
        }
    }
}
