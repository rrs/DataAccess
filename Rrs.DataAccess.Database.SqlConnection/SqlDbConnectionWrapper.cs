using System.Data;

namespace Rrs.DataAccess.Database.SqlConnection
{
    public class SqlDbConnectionWrapper : IDbConnectionWrapper
    {
        public SqlDbConnectionWrapper(IDbConnection connection)
        {
            Connection = connection;
        }
        public void Dispose()
        {
            Connection.Dispose();
        }

        public IDbConnection Connection { get; }
        public IDbTransaction Transaction { get; private set; }

        public IDbConnectionWrapper OpenTransaction()
        {
            Transaction = Connection.BeginTransaction();
            return this;
        }
    }
}
