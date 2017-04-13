namespace Rrs.DataAccess.Database
{
    public interface IDatabaseCommandExecutor
    {
        void Execute(IDatabaseCommand command);
    }
}
