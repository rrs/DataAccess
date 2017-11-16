using Rrs.DataAccess.DataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Rrs.DataAccess.DbCommand
{
    public static class Sql
    {
        public static CommandHelper Dynamic(string commandText, IDbConnection connection, IDbTransaction transaction = null) => new CommandHelper(CommandType.Text, commandText, connection, transaction);

        public static CommandHelper Sproc(string sprocName, IDbConnection connection, IDbTransaction transaction = null) => new CommandHelper(CommandType.StoredProcedure, sprocName, connection, transaction);

        public class CommandHelper
        {
            private IEnumerable<SqlParameter> _parameters;
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

            public T SingleRow<T>(Func<IDataReader, T> readerFunc)
            {
                return SingleRowReaderExecutor.Execute(_commandText, _dbCon, _dbTran, readerFunc, _parameters, _commandType);
            }

            public T SingleRow<T>()
            {
                return SingleRowReaderExecutor.Execute(_commandText, _dbCon, _dbTran, FastObjectReader.ManagedReader<T>(), _parameters, _commandType);
            }

            public IEnumerable<T> MultiRow<T>(Func<IDataReader, T> readerFunc)
            {
                return MultiRowReaderExecutor.Execute(_commandText, _dbCon, _dbTran, readerFunc, _parameters, _commandType);
            }

            public IEnumerable<T> MultiRow<T>()
            {
                return MultiRowReaderExecutor.Execute(_commandText, _dbCon, _dbTran, FastObjectReader.ManagedReader<T>(), _parameters, _commandType);
            }

            public IEnumerable<T> MultiValue<T>()
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
                _parameters = parameterObject.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Select(propertyInfo => new SqlParameter(propertyInfo.Name, ResolveType(propertyInfo.PropertyType), propertyInfo.GetValue(parameterObject, null)));
                return this;
            }

            public PrototypeReader<T> FromPrototype<T>(T prototype)
            {
                return new PrototypeReader<T>(this);
            }

            private Type ResolveType(Type t)
            {
                if (t.IsEnum)
                {
                    return t.GetEnumUnderlyingType();
                }
                return t;
            }
        }

        public class PrototypeReader<T>
        {
            private readonly CommandHelper _commandHelper;

            internal PrototypeReader(CommandHelper commandHelper)
            {
                _commandHelper = commandHelper;
            }

            public T SingleRow()
            {
                return _commandHelper.SingleRow(FastAnonymousObjectReader.ManagedReader<T>());
            }

            public IEnumerable<T> MultiRow()
            {
                return _commandHelper.MultiRow(FastAnonymousObjectReader.ManagedReader<T>());
            }
        }
    }
}
