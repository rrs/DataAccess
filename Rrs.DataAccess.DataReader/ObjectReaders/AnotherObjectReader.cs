using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Rrs.DataAccess.DataReader
{
    public static class AnotherObjectReader
    {
        public static T ReadObject<T>(this IDataRecord reader) where T : new()
        {
            return ObjectReaderIntertnal<T>.ReadObject(reader);
        }

        private static class ObjectReaderIntertnal<T> where T : new()
        {
            private static IEnumerable<IPropertyAccessor> PropertiesToWrite { get; }
            static ObjectReaderIntertnal()
            {
                PropertiesToWrite = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.CanWrite)
                    .Select(CreateAccessor);
            }

            internal static T ReadObject(IDataRecord reader)
            {
                var o = new T();

                foreach (var props in PropertiesToWrite)
                {
                    props.SetValue(o, reader.Read<object>(props.PropertyInfo.Name));
                }
                return o;
            }

            private static IPropertyAccessor CreateAccessor(PropertyInfo propertyInfo)
            {
                var genType = typeof(PropertyWrapper<,>).MakeGenericType(propertyInfo.DeclaringType, propertyInfo.PropertyType);
                return (IPropertyAccessor)Activator.CreateInstance(genType, propertyInfo);
            }
        }

        internal interface IPropertyAccessor
        {
            PropertyInfo PropertyInfo { get; }
            void SetValue(object source, object value);
        }

        internal class PropertyWrapper<TObject, TValue> : IPropertyAccessor where TObject : class
        {
            public PropertyInfo PropertyInfo { get; }
            private readonly Action<TObject, TValue> _setter;

            public PropertyWrapper(PropertyInfo propertyInfo)
            {
                PropertyInfo = propertyInfo;
                var setterInfo = propertyInfo.GetSetMethod(true);

                _setter = (Action<TObject, TValue>)Delegate.CreateDelegate(typeof(Action<TObject, TValue>), setterInfo);
            }

            void IPropertyAccessor.SetValue(object source, object value)
            {
                _setter(source as TObject, (TValue)value);
            }
        }
    }
}
