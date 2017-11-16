using System;
using System.Collections.Generic;
using System.Data;

namespace Rrs.DataAccess.DbCommand
{
    public static class MultiValueReaderExecutor
    {
        private static IEnumerable<T> MultiValueReaderCommand<T>(IDbCommand command)
        {
            using (var reader = command.ExecuteReader())
            {
                var list = new List<T>();

                while (reader.Read())
                {
                    var value = reader[0];
                    list.Add(value == DBNull.Value ? default(T) : (T)value);
                }
                return list;
            }
        }

        public static IEnumerable<T> Execute<T>(string commandText, IDbConnection connection, IDbTransaction transaction, IEnumerable<SqlParameter> parameters = null, CommandType commandType = CommandType.Text)
        {
            return CommandExecutor.Execute(commandText, connection, transaction, MultiValueReaderCommand<T>, parameters, commandType);
        }
    }
}
