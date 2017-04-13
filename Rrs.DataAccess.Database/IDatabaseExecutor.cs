using System;

namespace Rrs.DataAccess.Database
{
    public interface IDatabaseExecutor : IDatabaseCommandExecutor, IDatabaseQueryExecutor
    {
        void ExecuteInTransaction(Action<IDatabaseExecutorTransaction> executeFunc);
        IDatabaseExecutorTransaction NewTransaction();
    }
}
