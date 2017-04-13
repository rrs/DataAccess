namespace Rrs.DataAccess.Database
{
    public interface IDbConnectionWrapperFactory
    {
        IDbConnectionWrapper OpenConnection();
    }
}
