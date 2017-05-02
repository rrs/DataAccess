using Rrs.DataAccess.DataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Rrs.DataAccess.DbCommand
{
    public static class Sql
    {
        public static CommandHelper Dynamic(string commandText, IDbConnection connection, IDbTransaction transaction = null) => new CommandHelper(CommandType.Text, commandText, connection, transaction);

        public static CommandHelper Sproc(string sprocName, IDbConnection connection, IDbTransaction transaction = null) => new CommandHelper(CommandType.StoredProcedure, sprocName, connection, transaction);

        public class CommandHelper
        {
            private IEnumerable<Tuple<string, object>> _parameters;
            private readonly CommandType _commandType;
            private readonly string _commandText;
            private readonly IDbConnection _dbCon;
            private readonly IDbTransaction _dbTran;

            public CommandHelper(CommandType commandType, string commandText, IDbConnection dbCon, IDbTransaction dbTran)
            {
                _commandType = commandType;
                _commandText = commandText;
                _dbCon = dbCon;
                _dbTran = dbTran;
            }

            public int NonQuery()
            {
                return NonQueryExecutor.Execute(_commandText, _dbCon, _dbTran, _parameters, _commandType);
            }

            public T Scalar<T>()
            {
                return ScalarExecutor.Execute<T>(_commandText, _dbCon, _dbTran, _parameters, _commandType);
            }

            public T SingleRowReader<T>(Func<IDataReader, T> readerFunc)
            {
                return DbCommand.SingleRowReaderExecutor.Execute(_commandText, _dbCon, _dbTran, readerFunc, _parameters, _commandType);
            }

            public T SingleRowReader<T>()
            {
                return SingleRowReaderExecutor.Execute(_commandText, _dbCon, _dbTran, FastObjectReader.ManagedReader<T>(), _parameters, _commandType);
            }

            public IEnumerable<T> MultiRowReader<T>(Func<IDataReader, T> readerFunc)
            {
                return MultiRowReaderExecutor.Execute(_commandText, _dbCon, _dbTran, readerFunc, _parameters, _commandType);
            }

            public IEnumerable<T> MultiRowReader<T>()
            {
                return MultiRowReaderExecutor.Execute(_commandText, _dbCon, _dbTran, FastObjectReader.ManagedReader<T>(), _parameters, _commandType);
            }

            public IEnumerable<T> MultiValueReader<T>()
            {
                return MultiValueReaderExecutor.Execute<T>(_commandText, _dbCon, _dbTran, _parameters, _commandType);
            }

            public DataTable ToDataTable()
            {
                return DataTableReaderExecutor.Execute(_commandText, _dbCon, _dbTran, _parameters, _commandType);
            }

            public DataRow ToDataRow()
            {
                return DataRowReaderExecutor.Execute(_commandText, _dbCon, _dbTran, _parameters, _commandType);
            }

            public IEnumerable<Dictionary<string, object>> ToDictionary()
            {
                return DictionaryReaderExecutor.Execute(_commandText, _dbCon, _dbTran, _parameters, _commandType);
            }

            public CommandHelper WithParameters(object parameterObject)
            {
                _parameters = parameterObject.ToParameterList();
                return this;
            }

            public CommandHelper WithParameters(IEnumerable<Tuple<string, object>> parameters)
            {
                _parameters = parameters;
                return this;
            }
            public CommandHelper WithParameters(IEnumerable<KeyValuePair<string, object>> parameters)
            {
                _parameters = parameters.Select(o => new Tuple<string, object>(o.Key, o.Value));
                return this;
            }

            public PrototypeReader<T> FromPrototype<T>(T prototype)
            {
                return new PrototypeReader<T>(this);
            }
        }

        public class PrototypeReader<T>
        {
            private readonly CommandHelper _commandHelper;

            internal PrototypeReader(CommandHelper commandHelper)
            {
                _commandHelper = commandHelper;
            }

            public T SingleRowReader()
            {
                return _commandHelper.SingleRowReader(FastAnonymousObjectReader.ManagedReader<T>());
            }

            public IEnumerable<T> MultiRowReader()
            {
                return _commandHelper.MultiRowReader(FastAnonymousObjectReader.ManagedReader<T>());
            }
        }
    }
}
