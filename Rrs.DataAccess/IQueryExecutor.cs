namespace Rrs.DataAccess
{
    public interface IQueryExecutor
    {
        T Execute<T>(IQuery<T> query);
    }
}
