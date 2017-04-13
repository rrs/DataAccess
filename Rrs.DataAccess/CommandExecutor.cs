using System;

namespace Rrs.DataAccess
{
    public class CommandExecutor : ICommandExecutor
    {
        public void Execute(ICommand command)
        {
            try
            {
                command.Execute();
            }
            catch(Exception e)
            {
                throw new DataAccessException($"Exception occured whilst executing a query. Message {e.Message}", e);
            }
        }
    }
}
