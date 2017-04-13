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
        /// Fastest implementation of reading objects from an IDataReader
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        private static Func<IDataReader, T> ReaderFunc(IDataReader dataReader)
        {
            var exps = new List<Expression>();

            var paramExp = Expression.Parameter(typeof(IDataRecord));

            var targetExp = Expression.Variable(typeof(T));
            exps.Add(Expression.Assign(targetExp, Expression.New(targetExp.Type)));

            //does int based lookup
            var indexerInfo = typeof(IDataRecord).GetProperty("Item", new[] { typeof(int) });

            var columns = Enumerable.Range(0, dataReader.FieldCount).Select(i => new { i, name = dataReader.GetName(i), type = dataReader.GetFieldType(i) });
            foreach (var column in columns)
            {
                var property = targetExp.Type.GetProperty(column.name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (property == null) continue;

                var columnIndexExp = Expression.Constant(column.i);
                var cellExp = Expression.MakeIndex(paramExp, indexerInfo, new[] { columnIndexExp });
                var cellValueExp = Expression.Variable(typeof(object));
                var convertExp = Expression.Condition(
                    Expression.Equal(cellValueExp, Expression.Constant(DBNull.Value)),
                    Expression.Default(property.PropertyType),
                    Expression.Convert(Expression.Convert(cellValueExp, column.type), property.PropertyType));
                var cellValueReadExp = Expression.Block(new[] { cellValueExp }, Expression.Assign(cellValueExp, cellExp), convertExp);
                var bindExp = Expression.Assign(Expression.Property(targetExp, property), cellValueReadExp);
                exps.Add(bindExp);
            }

            exps.Add(targetExp);
            return Expression.Lambda<Func<IDataReader, T>>(Expression.Block(new[] { targetExp }, exps), paramExp).Compile();
        }

        internal T ReadObject(IDataReader dataReader)
        {
            if (_converter == null) _converter = ReaderFunc(dataReader);
            return _converter(dataReader);
        }
    }
}
