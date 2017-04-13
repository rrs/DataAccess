namespace Rrs.DataAccess.Database
{
    public interface IDatabaseCommand
    {
        void Execute(IDbConnectionWrapper c);
    }
}
