using System;
using System.Collections.Generic;

namespace Rrs.Common
{
    /// <summary>
    /// Allows shorthand list initialization of tuples. i.e. new TupleList{T1,T2} { {value1, value2}, {value3, value4} }
    /// See overloading Add method on list objects
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    public class TupleList<T1, T2> : List<Tuple<T1, T2>>
    {
        public void Add(T1 item1, T2 item2)
        {
            Add(new Tuple<T1, T2>(item1, item2));
        }
    }
}
