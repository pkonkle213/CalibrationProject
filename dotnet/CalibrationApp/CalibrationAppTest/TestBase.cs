using System.Data.SqlClient;
using System.Transactions;

namespace CalibrationAppTest
{
    [TestClass]
    public class TestBase
    {
        private TransactionScope trans;

        protected string ConnectionString
        {
            get
            {
                return "Server=DESKTOP-J9CSEAD;Database=calibration_database;Trusted_Connection=True;";
            }
        }

        [TestInitialize]
        public void Setup()
        {
            trans = new TransactionScope(); // BEGIN TRANSACTION

            // Get the SQL Script to run
            string sql = File.ReadAllText("C:\\Users\\Phillip\\source\\repos\\CalibrationApp\\dotnet\\CalibrationApp\\CalibrationAppTest\\setup.sql");

            // Execute the script
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.ExecuteNonQuery();
            }
        }

        [TestCleanup]
        public void Reset()
        {
            trans?.Dispose(); // ROLLBACK TRANSACTION
        }

        protected int GetRowCount(string tableName)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand($"SELECT COUNT(*) FROM {tableName}", conn);
                int count = Convert.ToInt32(cmd.ExecuteScalar());

                return count;
            }
        }
    }
}