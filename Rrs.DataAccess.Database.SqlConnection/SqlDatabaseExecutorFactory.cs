namespace Rrs.DataAccess.Database.SqlConnection
{
    public static class SqlDatabaseExecutorFactory
    {
        public static DatabaseExecutor New(string connectionString, ICommandExecutor commandExecutor = null, IQueryExecutor queryExecutor = null)
        {
            var f = new SqlDbConnectionWrapperFactory(connectionString);
            return new DatabaseExecutor(f, commandExecutor ?? new CommandExecutor(), queryExecutor ?? new QueryExecutor());
        }
    }
}
