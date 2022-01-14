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

        public int CreateCalibration(Calibration calibration)
        {
            int id = -1;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                const string sql = "INSERT INTO Calibrations (calibration_date,contact_type,contact_id,tm_first_name,tm_last_name,form_id,isOpen) " +
                    "VALUES (@date,@type,@id,@first,@last,1,1); " +
                    "SELECT @@IDENTITY";

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.Parameters.AddWithValue("@date", calibration.Date);
                    command.Parameters.AddWithValue("@type", calibration.Type);
                    command.Parameters.AddWithValue("@id", calibration.ContactId);
                    command.Parameters.AddWithValue("@first", calibration.FirstName);
                    command.Parameters.AddWithValue("@last", calibration.LastName);

                    id = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            return id;
        }

        public List<Calibration> GetOpenCalibrations()
        {
            List<Calibration> calibrations = new List<Calibration>();

            const string sql = "SELECT c.calibration_id,c.calibration_date,con.type,c.contact_id,c.tm_first_name,c.tm_last_name " +
                "FROM Calibrations c " +
                "INNER JOIN Contacts con ON con.contact_id = c.contact_type " +
                "ORDER BY c.calibration_date DESC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Calibration calibration = new Calibration();

                            calibration.Id = Convert.ToInt32(reader["c.calibration_id"]);
                            calibration.Date = Convert.ToDateTime(reader["c.calibration_date"]);
                            calibration.Type = Convert.ToString(reader["con.type"]);
                            calibration.ContactId = Convert.ToString(reader["c.contact_id"]);
                            calibration.FirstName = Convert.ToString(reader["c.tm_first_name"]);
                            calibration.LastName = Convert.ToString(reader["c.tm_last_name"]);

                            calibrations.Add(calibration);
                        }
                    }
                }
            }

            return calibrations;
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

    }
}
