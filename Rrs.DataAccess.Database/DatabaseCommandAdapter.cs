namespace Rrs.DataAccess.Database
{
    internal class DatabaseCommandAdapter : ICommand
    {
        private readonly IDatabaseCommand _command;
        private readonly IDbConnectionWrapper _connection;

        public DatabaseCommandAdapter(IDatabaseCommand command, IDbConnectionWrapper connection)
        {
            _command = command;
            _connection = connection;
        }

        public void Execute()
        {
            _command.Execute(_connection);
        }
    }
}
