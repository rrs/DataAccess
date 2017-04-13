using System;

namespace Rrs.DataAccess.Database
{
    public class DatabaseExecutor : IDatabaseExecutor
    {
        private readonly IDbConnectionWrapperFactory _connectionFactory;
        private readonly ICommandExecutor _commandExecutor;
        private readonly IQueryExecutor _queryExecutor;

        public DatabaseExecutor(IDbConnectionWrapperFactory connectionFactory, ICommandExecutor commandExecutor, IQueryExecutor queryExecutor)
        {
            _connectionFactory = connectionFactory;
            _commandExecutor = commandExecutor;
            _queryExecutor = queryExecutor;
        }

        public void Execute(IDatabaseCommand command)
        {
            using (var c = _connectionFactory.OpenConnection())
            {
                _commandExecutor.Execute(new DatabaseCommandAdapter(command, c));
            }
        }

        public T Execute<T>(IDatabaseQuery<T> query)
        {
            using (var c = _connectionFactory.OpenConnection())
            {
                return _queryExecutor.Execute(new DatabaseQueryAdapter<T>(query, c));
            }
        }

        public void ExecuteInTransaction(Action<IDatabaseExecutorTransaction> executeFunc)
        {
            using (var transaction = NewTransaction())
            {
                executeFunc(transaction);
                transaction.Commit();
            }
        }

        public IDatabaseExecutorTransaction NewTransaction()
        {
            return new DatabaseExecutorTransaction(_connectionFactory.OpenConnection().OpenTransaction(), _commandExecutor, _queryExecutor);
        }
    }
}
