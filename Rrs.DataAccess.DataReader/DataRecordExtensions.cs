using System;
using System.Data;

namespace Rrs.DataAccess.DataReader
{
    public static class DataRecordExtensions
    {
        /// <summary>
        /// Helper for reading sql columns, handles DBNull as default of the generic type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static T Read<T>(this IDataRecord reader, string column)
        {
            object item;
            try
            {
                item = reader[column];
            }
            catch (IndexOutOfRangeException e)
            {
                throw new DataReaderException($"Trying to read column '{column}' which does not exist", e);
            }

            if (item == DBNull.Value)
            {
                return default(T);
            }
            try
            {
                return (T)item;
            }
            catch (InvalidCastException e)
            {
                throw new DataReaderException($"Invalid cast exception column '{column}' is of type '{item.GetType()}' and specified cast is {typeof(T)}", e);
            }
        }

        /// <summary>
        /// Helper for reading sql columns, handles DBNull as default of the generic type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public static T Read<T>(this IDataRecord reader, int columnIndex)
        {
            object item;
            try
            {
                item = reader[columnIndex];
            }
            catch (IndexOutOfRangeException e)
            {
                throw new DataReaderException($"Trying to read column '{reader.GetName(columnIndex)}' which does not exist", e);
            }

            if (item == DBNull.Value)
            {
                return default(T);
            }
            try
            {
                return (T)item;
            }
            catch (InvalidCastException e)
            {
                throw new DataReaderException($"Invalid cast exception column '{reader.GetName(columnIndex)}' is of type '{item.GetType()}' and specified cast is {typeof(T)}", e);
            }
        }
    }
}
