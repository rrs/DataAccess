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
        public static T Execute<T>(string commandText, IDbConnection connection, IDbTransaction transaction, Func<IDbCommand, T> commandFunc, IEnumerable<Tuple<string, object>> parameters = null, CommandType commandType = CommandType.Text)
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
        private static void AddParameters(this IDbCommand command, IEnumerable<Tuple<string, object>> parameters)
        {
            foreach (var parameter in parameters ?? Enumerable.Empty<Tuple<string, object>>())
            {
                var p = command.CreateParameter();
                p.ParameterName = parameter.Item1;
                p.Value = parameter.Item2 ?? DBNull.Value;
                command.Parameters.Add(p);
            }
        }
    }
}
