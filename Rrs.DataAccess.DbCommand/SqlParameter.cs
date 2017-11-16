using System;

namespace Rrs.DataAccess.DbCommand
{
    public class SqlParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public Type ParameterType { get; set; }

        public SqlParameter(string name, Type pType, object value)
        {
            Name = name;
            ParameterType = pType;
            Value = value;
        }
    }
}
