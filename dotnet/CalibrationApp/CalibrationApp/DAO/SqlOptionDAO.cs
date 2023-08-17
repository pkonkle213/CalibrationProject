namespace CalibrationApp.DAO
{
    public class SqlOptionDAO : IOptionDAO
    {
        private readonly string connectionString;
        public SqlOptionDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
    }
}
