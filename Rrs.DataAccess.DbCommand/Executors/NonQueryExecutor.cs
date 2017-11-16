using System;
using System.Collections.Generic;
using System.Data;

namespace Rrs.DataAccess.DbCommand
{
    public static class NonQueryExecutor
    {
        private static int NonQueryCommand(IDbCommand commmand) => commmand.ExecuteNonQuery();

        public static int Execute(string commandText, IDbConnection connection, IDbTransaction transaction, IEnumerable<SqlParameter> parameters = null, CommandType commandType = CommandType.Text)
        {
            return CommandExecutor.Execute(commandText, connection, transaction, NonQueryCommand, parameters, commandType);
        }
    }
}
