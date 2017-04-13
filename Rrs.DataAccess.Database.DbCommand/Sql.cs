using System.Data;
using static Rrs.DataAccess.DbCommand.Sql;

namespace Rrs.DataAccess.Database.DbCommand
{
    public static class Sql
    {
        public static CommandHelper Dynamic(string commandText, IDbConnectionWrapper wrapper) => new CommandHelper(CommandType.Text, commandText, wrapper.Connection, wrapper.Transaction);

        public static CommandHelper Sproc(string sprocName, IDbConnectionWrapper wrapper) => new CommandHelper(CommandType.StoredProcedure, sprocName, wrapper.Connection, wrapper.Transaction);
    }
}
