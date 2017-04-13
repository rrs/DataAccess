using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Rrs.DataAccess.DataReader
{
    public static class ObjectReader
    {
        public static T ReadObject<T>(this IDataRecord reader) where T : new()
        {
            return ObjectReaderIntertnal<T>.ReadObject(reader);
        }

        private static class ObjectReaderIntertnal<T> where T : new()
        {
            private static IEnumerable<PropertyInfo> PropertiesToWrite { get; }

            static ObjectReaderIntertnal()
            {
                PropertiesToWrite = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.CanWrite);
            }

            internal static T ReadObject(IDataRecord reader)
            {
                var o = new T();

                foreach (var propertyInfo in PropertiesToWrite)
                {
                    propertyInfo.SetValue(o, reader.Read<object>(propertyInfo.Name), null);
                }
                return o;
            }
        }
    }
}
