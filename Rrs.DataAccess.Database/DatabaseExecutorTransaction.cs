using System;

namespace Rrs.DataAccess.Database
{
    public class DatabaseExecutorTransaction : IDatabaseExecutorTransaction
    {
        private readonly IDbConnectionWrapper _connection;
        private readonly ICommandExecutor _commandExecutor;
        private readonly IQueryExecutor _queryExecutor;

        public DatabaseExecutorTransaction(IDbConnectionWrapper connection, ICommandExecutor commandExecutor, IQueryExecutor queryExecutor)
        {
            _connection = connection;
            _commandExecutor = commandExecutor;
            _queryExecutor = queryExecutor;
        }

        public void Execute(IDatabaseCommand command)
        {
            _commandExecutor.Execute(new DatabaseCommandAdapter(command, _connection));
        }

        public T Execute<T>(IDatabaseQuery<T> query)
        {
            return _queryExecutor.Execute(new DatabaseQueryAdapter<T>(query, _connection));
        }

        public void Commit()
        {
            _connection.Transaction.Commit();
        }

        public void RollBack()
        {
            _connection.Transaction.Rollback();
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
