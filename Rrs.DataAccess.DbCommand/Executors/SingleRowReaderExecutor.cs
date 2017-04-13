using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Rrs.DataAccess.DbCommand
{
    public static class SingleRowReaderExecutor
    {
        /// <summary>
        /// Reads a stored procedure that returns a single line and reads it with the given reader function
        /// </summary>
        public static T Execute<T>(string commandText, IDbConnection connection, IDbTransaction transaction, Func<IDataReader, T> readerFunc, IEnumerable<Tuple<string, object>> parameters = null, CommandType commandType = CommandType.Text)
        {
            return MultiRowReaderExecutor.Execute(commandText, connection, transaction, readerFunc, parameters, commandType).FirstOrDefault();
        }
    }
}
