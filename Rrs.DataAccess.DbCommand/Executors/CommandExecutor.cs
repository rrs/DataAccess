using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Rrs.DataAccess.DbCommand
{
    public static class CommandExecutor
    {
        /// <summary>
        /// Executes a stored procedure on the given connection string. Adds optional parameters. Uses a reader function to reader the sql return values
        /// </summary>
        public static T Execute<T>(string commandText, IDbConnection connection, IDbTransaction transaction, Func<IDbCommand, T> commandFunc, IEnumerable<SqlParameter> parameters = null, CommandType commandType = CommandType.Text)
        {
            try
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    command.CommandType = commandType;
                    command.Transaction = transaction;

                    command.AddParameters(parameters);

                    return commandFunc(command);
                }
            }
            catch (Exception e)
            {
                throw new CommandException($"Error when executing query. Command Type {commandType}. Command Text {commandText}. Message: {e.Message}", e);
            }
        }

        /// <summary>
        /// Add parameters to the command handling nulls as DBNull
        /// </summary>
        private static void AddParameters(this IDbCommand command, IEnumerable<SqlParameter> parameters)
        {
            foreach (var parameter in parameters ?? Enumerable.Empty<SqlParameter>())
            {
                var p = command.CreateParameter();
                p.ParameterName = parameter.Name;
                p.Value = parameter.Value ?? DBNull.Value;
                p.DbType = _typeMap[parameter.ParameterType];
                command.Parameters.Add(p);
            }
        }

        private static readonly Dictionary<Type, DbType> _typeMap = new Dictionary<Type, DbType>
        {
            [typeof(byte)] = DbType.Byte,
            [typeof(sbyte)] = DbType.SByte,
            [typeof(short)] = DbType.Int16,
            [typeof(ushort)] = DbType.UInt16,
            [typeof(int)] = DbType.Int32,
            [typeof(uint)] = DbType.UInt32,
            [typeof(long)] = DbType.Int64,
            [typeof(ulong)] = DbType.UInt64,
            [typeof(float)] = DbType.Single,
            [typeof(double)] = DbType.Double,
            [typeof(decimal)] = DbType.Decimal,
            [typeof(bool)] = DbType.Boolean,
            [typeof(string)] = DbType.String,
            [typeof(char)] = DbType.StringFixedLength,
            [typeof(Guid)] = DbType.Guid,
            [typeof(DateTime)] = DbType.DateTime,
            [typeof(DateTimeOffset)] = DbType.DateTimeOffset,
            [typeof(byte[])] = DbType.Binary,
            [typeof(byte?)] = DbType.Byte,
            [typeof(sbyte?)] = DbType.SByte,
            [typeof(short?)] = DbType.Int16,
            [typeof(ushort?)] = DbType.UInt16,
            [typeof(int?)] = DbType.Int32,
            [typeof(uint?)] = DbType.UInt32,
            [typeof(long?)] = DbType.Int64,
            [typeof(ulong?)] = DbType.UInt64,
            [typeof(float?)] = DbType.Single,
            [typeof(double?)] = DbType.Double,
            [typeof(decimal?)] = DbType.Decimal,
            [typeof(bool?)] = DbType.Boolean,
            [typeof(char?)] = DbType.StringFixedLength,
            [typeof(Guid?)] = DbType.Guid,
            [typeof(DateTime?)] = DbType.DateTime,
            [typeof(DateTimeOffset?)] = DbType.DateTimeOffset,
            //[typeof(System.Data.Linq.Binary)] = DbType.Binary,
        };
    }
}
