using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace Rrs.DataAccess.DataReader
{
    public static class FastAnonymousObjectReader
    {
        public static Func<IDataReader, T> ManagedReader<T>()
        {
            var fastReader = new FastAnonymousObjectReader<T>();

            return r => fastReader.ReadObject(r);
        }
    }

    public class FastAnonymousObjectReader<T>
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

            var ctor = typeof(T).GetConstructors().First();
            var columns = Enumerable.Range(0, dataReader.FieldCount).Select(i => new { i, name = dataReader.GetName(i), type = dataReader.GetFieldType(i) });

            var ctorParamsInfo = ctor.GetParameters();

            var ctorParams = new List<Expression>();

            var indexerInfo = typeof(IDataRecord).GetProperty("Item", new[] { typeof(int) });
            foreach (var param in ctorParamsInfo.Select((p, i) => new { p, i }))
            {
                var column = columns.FirstOrDefault(o => string.Equals(o.name, param.p.Name, StringComparison.OrdinalIgnoreCase));
                if (column == null) continue;

                var columnIndexExp = Expression.Constant(column.i);
                var cellExp = Expression.MakeIndex(paramExp, indexerInfo, new[] { columnIndexExp });
                var cellValueExp = Expression.Variable(typeof(object));
                var convertExp = Expression.Condition(
                    Expression.Equal(cellValueExp, Expression.Constant(DBNull.Value)),
                    Expression.Default(param.p.ParameterType),
                    Expression.Convert(Expression.Convert(cellValueExp, column.type), param.p.ParameterType));
                var cellValueReadExp = Expression.Block(new[] { cellValueExp }, Expression.Assign(cellValueExp, cellExp), convertExp);
                ctorParams.Add(cellValueReadExp);
            }

            exps.Add(Expression.Assign(targetExp, Expression.New(ctor, ctorParams)));

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
