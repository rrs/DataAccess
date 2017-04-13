using Rrs.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rrs.DataAccess.DataReader
{
    public static class ObjectExtensions
    {
        public static IEnumerable<Tuple<string, object>> ToParameterList(this object o)
        {
            var propertyDictionary = o.ToPropertyDictionary();

            return propertyDictionary.Select(p => new Tuple<string, object>("@" + p.Key, p.Value));
        }
    }
}
