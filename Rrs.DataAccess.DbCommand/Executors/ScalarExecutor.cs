using System;
using System.Collections.Generic;
using System.Data;

namespace Rrs.DataAccess.DbCommand
{
    public static class ScalarExecutor
    {
        private static T ScalarCommand<T>(IDbCommand command)
        {
            var result = command.ExecuteScalar();
            if (result == DBNull.Value) return default(T);

            return (T)result;
        }

        /// <summary>
        /// Executes a sql scalar stored procedure, returns the value casted to type {T}
        /// </summary>
        public static T Execute<T>(string commandText, IDbConnection connection, IDbTransaction transaction, IEnumerable<SqlParameter> parameters = null, CommandType commandType = CommandType.Text)
        {
            return CommandExecutor.Execute(commandText, connection, transaction, ScalarCommand<T>, parameters, commandType);
        }
    }
}
