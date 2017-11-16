using System;

namespace Rrs.DataAccess.Database
{
    public interface IDatabaseExecutor : IDatabaseCommandExecutor, IDatabaseQueryExecutor
    {
        void ExecuteInTransaction(Action<IDatabaseExecutorTransaction> executeFunc);
        void ExecuteInTransaction(params IDatabaseCommand[] commands);
        IDatabaseExecutorTransaction NewTransaction();
    }
}
