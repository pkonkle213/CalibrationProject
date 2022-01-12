namespace CalibrationApp.DAO
{
    public class SqlCalibrationDAO : ICalibrationDAO
    {
        private readonly string connectionString;

        public SqlCalibrationDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
    }
}
