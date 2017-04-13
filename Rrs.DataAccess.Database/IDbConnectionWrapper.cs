using System;
using System.Data;

namespace Rrs.DataAccess.Database
{
    public interface IDbConnectionWrapper : IDisposable
    {
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }
        IDbConnectionWrapper OpenTransaction();
    }
}
