using System;
using System.Collections.Generic;
using System.Data;

namespace Rrs.DataAccess.DbCommand
{
    public static class MultiRowReaderExecutor
    {
        private static IEnumerable<T> MultiRowReaderCommand<T>(IDbCommand command, Func<IDataReader, T> readerFunc)
        {
            using (var reader = command.ExecuteReader())
            {
                var list = new List<T>();

                while (reader.Read())
                {
                    T item = readerFunc(reader);
                    list.Add(item);
                }
                return list;
            }
        }

        public static IEnumerable<T> Execute<T>(string commandText, IDbConnection connection, IDbTransaction transaction, Func<IDataReader, T> readerFunc, IEnumerable<Tuple<string, object>> parameters = null, CommandType commandType = CommandType.Text)
        {
            Func<IDbCommand, IEnumerable<T>> commandFunc = command => MultiRowReaderCommand(command, readerFunc);

            return CommandExecutor.Execute(commandText, connection, transaction, commandFunc, parameters, commandType);
        }
    }
}
