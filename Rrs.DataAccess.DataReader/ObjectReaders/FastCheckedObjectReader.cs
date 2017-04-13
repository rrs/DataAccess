using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Rrs.DataAccess.DataReader
{
    public static class FastCheckedObjectReader
    {
        public static Func<IDataReader, T> ManagedReader<T>()
        {
            var fastReader = new FastObjectReader<T>();

            return r => fastReader.ReadObject(r);
        }
    }

    public class FastCheckedObjectReader<T>
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

            var paramExp = Expression.Parameter(typeof(IDataRecord));

            var targetExp = Expression.Variable(typeof(T));
            exps.Add(Expression.Assign(targetExp, Expression.New(targetExp.Type)));

            var columns = Enumerable.Range(0, dataReader.FieldCount).Select(i => new { i, name = dataReader.GetName(i), type = dataReader.GetFieldType(i) });
            foreach (var column in columns)
            {
                var property = targetExp.Type.GetProperty(column.name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (property == null) continue;

                var readExp = Expression.Call(typeof(DataRecordExtensions), nameof(DataRecordExtensions.Read), new[] { column.type }, paramExp, Expression.Constant(column.i));
                var convertExp = Expression.Convert(readExp, property.PropertyType);
                var bindExp = Expression.Assign(Expression.Property(targetExp, property), convertExp);
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
