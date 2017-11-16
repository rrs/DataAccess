using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Rrs.DataAccess.DbCommand
{
    public static class DictionaryReaderExecutor
    {
        private static IEnumerable<Dictionary<string, object>> DictionaryReaderCommand(IDbCommand command)
        {
            using (var reader = command.ExecuteReader())
            {
                var list = new List<Dictionary<string, object>>();

                while (reader.Read())
                {
                    var row = Enumerable.Range(0, reader.FieldCount).ToDictionary(i => reader.GetName(i), i => reader.GetValue(i));
                    list.Add(row);
                }
                return list;
            }
        }

        public static IEnumerable<Dictionary<string, object>> Execute(string commandText, IDbConnection connection, IDbTransaction transaction, IEnumerable<SqlParameter> parameters = null, CommandType commandType = CommandType.Text)
        {
            return CommandExecutor.Execute(commandText, connection, transaction, DictionaryReaderCommand, parameters, commandType);
        }
    }
}
