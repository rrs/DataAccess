namespace Rrs.DataAccess.Database.SqlConnection
{
    public class SqlDbConnectionWrapperFactory : IDbConnectionWrapperFactory
    {
        private readonly string _connectionString;

        public SqlDbConnectionWrapperFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnectionWrapper OpenConnection()
        {
            var c = new System.Data.SqlClient.SqlConnection(_connectionString);
            c.Open();
            return new SqlDbConnectionWrapper(c);
        }
    }
}
