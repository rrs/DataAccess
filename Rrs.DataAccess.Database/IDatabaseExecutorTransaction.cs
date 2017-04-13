using System;

namespace Rrs.DataAccess.Database
{
    public interface IDatabaseExecutorTransaction : IDatabaseCommandExecutor, IDatabaseQueryExecutor, IDisposable
    {
        void Commit();
        void RollBack();
    }
}
