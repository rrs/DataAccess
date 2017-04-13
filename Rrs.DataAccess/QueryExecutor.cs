using System;

namespace Rrs.DataAccess
{
    public class QueryExecutor : IQueryExecutor
    {
        public T Execute<T>(IQuery<T> query)
        {
            try
            {
                return query.Execute();
            }
            catch (Exception e)
            {
                throw new DataAccessException($"Exception occured whilst executing a query. Message {e.Message}", e);
            }
        }
    }
}
