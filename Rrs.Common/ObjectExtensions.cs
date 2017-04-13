using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rrs.Common
{
    public static class ObjectExtensions
    {
        public static Dictionary<string, object> ToPropertyDictionary(this object o)
        {
            return o.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(propertyInfo => propertyInfo.Name, propertyInfo => propertyInfo.GetValue(o, null));
        }
    }
}
