namespace YellowSquad.Anthill.Meta
{
    internal class UpdateSessionViewCommand : IButtonCommand
    {
        private readonly IButtonCommand _command;
        private readonly ISession _session;
        private readonly ISessionView _view;

        public UpdateSessionViewCommand(IButtonCommand command, ISession session, ISessionView view)
        {
            _command = command;
            _session = session;
            _view = view;
        }

        public bool CanExecute => _command.CanExecute;
        
        public void Execute()
        {
            _command.Execute();
            _session.Visualize(_view);
        }
    }
}