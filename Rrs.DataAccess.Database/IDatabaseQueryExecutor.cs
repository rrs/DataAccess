namespace Rrs.DataAccess.Database
{
    public interface IDatabaseQueryExecutor
    {
        T Execute<T>(IDatabaseQuery<T> query);
    }
}
