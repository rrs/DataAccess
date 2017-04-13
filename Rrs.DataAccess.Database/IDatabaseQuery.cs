namespace Rrs.DataAccess.Database
{
    public interface IDatabaseQuery<T>
    {
        T Execute(IDbConnectionWrapper c);
    }
}
