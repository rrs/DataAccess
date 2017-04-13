namespace Rrs.DataAccess.Database
{
    internal class DatabaseQueryAdapter<T> : IQuery<T>
    {
        private readonly IDatabaseQuery<T> _query;
        private readonly IDbConnectionWrapper _connection;

        public DatabaseQueryAdapter(IDatabaseQuery<T> query, IDbConnectionWrapper connection)
        {
            _query = query;
            _connection = connection;
        }

        public T Execute()
        {
            return _query.Execute(_connection);
        }
    }
}
