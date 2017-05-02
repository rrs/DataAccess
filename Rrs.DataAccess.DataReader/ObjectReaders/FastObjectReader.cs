using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Rrs.DataAccess.DataReader
{
    public static class FastObjectReader
    {
        public static Func<IDataReader, T> ManagedReader<T>()
        {
            var fastReader = new FastObjectReader<T>();

            return r => fastReader.ReadObject(r);
        }
    }

    public class FastObjectReader<T>
    {
        private Func<IDataReader, T> _converter;

        /// <summary>
        /// Tad slower but uses the read extension which offers better info on failures
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        private Func<IDataReader, T> ReaderFunc(IDataReader dataReader)
        {
            var exps = new List<Expression>();

            var readerExp = Expression.Parameter(typeof(IDataReader));

            var targetExp = Expression.Variable(typeof(T));
            exps.Add(Expression.Assign(targetExp, Expression.New(targetExp.Type)));

            var columns = Enumerable.Range(0, dataReader.FieldCount).Select(i => new { i, name = dataReader.GetName(i), type = dataReader.GetFieldType(i) });

            var dbNullMethod = typeof(IDataRecord).GetMethod(nameof(IDataRecord.IsDBNull), new[] { typeof(int) });

            foreach (var column in columns)
            {
                var property = targetExp.Type.GetProperty(column.name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (property == null) continue;

                var nullCheck = Expression.Call(readerExp, dbNullMethod, Expression.Constant(column.i));
                var readMethod = ReadMethods[column.type];
                var readExp = Expression.Call(readerExp, readMethod, Expression.Constant(column.i));
                var convertExp = ConvertExpression(readExp, column.type, property.PropertyType);
                var readIfNotNullExp = Expression.Condition(
                    nullCheck,
                    Expression.Default(property.PropertyType),
                    convertExp);
                var bindExp = Expression.Assign(Expression.Property(targetExp, property), readIfNotNullExp);
                exps.Add(bindExp);
            }

            exps.Add(targetExp);
            return Expression.Lambda<Func<IDataReader, T>>(Expression.Block(new[] { targetExp }, exps), readerExp).Compile();
        }

        private Expression ConvertExpression(Expression readExp, Type columnType, Type propertyType)
        {
            if (columnType == typeof(int) && propertyType == typeof(bool))
            {
                return Expression.Equal(readExp, Expression.Constant(1));
            }
            return Expression.Convert(readExp, propertyType);
        }

        internal T ReadObject(IDataReader dataReader)
        {
            if (_converter == null) _converter = ReaderFunc(dataReader);
            return _converter(dataReader);
        }

        private Dictionary<Type, MethodInfo> ReadMethods = new Dictionary<Type, MethodInfo>
        {
            { typeof(bool), typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetBoolean), new[] { typeof(int) }) },
            { typeof(char), typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetChar), new[] { typeof(int) }) },
            { typeof(string), typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetString), new[] { typeof(int) }) },
            { typeof(byte), typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetByte), new[] { typeof(int) }) },
            { typeof(short), typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetInt16), new[] { typeof(int) }) },
            { typeof(int), typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetInt32), new[] { typeof(int) }) },
            { typeof(long), typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetInt64), new[] { typeof(int) }) },
            { typeof(float), typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetFloat), new[] { typeof(int) }) },
            { typeof(double), typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetDouble), new[] { typeof(int) }) },
            { typeof(decimal), typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetDecimal), new[] { typeof(int) }) },
            { typeof(DateTime), typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetDateTime), new[] { typeof(int) }) },
            { typeof(Guid), typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetGuid), new[] { typeof(int) }) },
            { typeof(byte[]), typeof(IDataRecord).GetMethod(nameof(IDataRecord.GetValue), new[] { typeof(int) }) },
        };
    }
}
