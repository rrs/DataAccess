using System;

namespace Rrs.DataAccess.DataReader
{
    public class DataReaderException : Exception
    {
        public DataReaderException() { }

        public DataReaderException(string message) : base(message) { }

        public DataReaderException(string message, Exception innerException) : base(message, innerException) { }
    }
}
