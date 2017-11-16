using System;
using System.Collections.Generic;
using System.Data;

namespace Rrs.DataAccess.DbCommand
{
    public static class DataRowReaderExecutor
    {
        public static DataRow Execute(string commandText, IDbConnection connection, IDbTransaction transaction, IEnumerable<SqlParameter> parameters = null, CommandType commandType = CommandType.Text)
        {
            return DataTableReaderExecutor.Execute(commandText, connection, transaction, parameters, commandType).Rows[0];
        }
    }
}
