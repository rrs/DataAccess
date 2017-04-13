namespace Rrs.DataAccess
{
    public interface ICommandExecutor
    {
        void Execute(ICommand command);
    }
}
