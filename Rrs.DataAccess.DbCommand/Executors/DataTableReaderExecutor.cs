using System;
using System.Collections.Generic;
using System.Data;

namespace Rrs.DataAccess.DbCommand
{
    public static class DataTableReaderExecutor
    {
        private static DataTable DataTableReaderCommand(IDbCommand command)
        {
            var reader = command.ExecuteReader();
            var dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable;
        }

        public static DataTable Execute(string commandText, IDbConnection connection, IDbTransaction transaction, IEnumerable<SqlParameter> parameters = null, CommandType commandType = CommandType.Text)
        {
            return CommandExecutor.Execute(commandText, connection, transaction, DataTableReaderCommand, parameters, commandType);
        }
    }
}
