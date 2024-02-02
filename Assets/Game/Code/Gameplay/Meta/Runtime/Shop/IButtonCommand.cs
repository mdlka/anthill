using System;

namespace YellowSquad.Anthill.Meta
{
    internal interface IButtonCommand
    {
        bool CanExecute { get; }
        void Execute();
    }

    internal class AddDiggerCommand : IButtonCommand
    {
        private readonly ISession _session;

        public AddDiggerCommand(ISession session)
        {
            _session = session;
        }

        public bool CanExecute => _session.CanAddDigger;
        
        public void Execute()
        {
            if (CanExecute == false)
                throw new InvalidOperationException();
            
            _session.AddDigger();
        }
    }
    
    internal class AddLoaderCommand : IButtonCommand
    {
        private readonly ISession _session;

        public AddLoaderCommand(ISession session)
        {
            _session = session;
        }

        public bool CanExecute => _session.CanAddLoader;
        
        public void Execute()
        {
            if (CanExecute == false)
                throw new InvalidOperationException();
            
            _session.AddLoader();
        }
    }
    
    internal class IncreaseSpeedCommand : IButtonCommand
    {
        private readonly ISession _session;

        public IncreaseSpeedCommand(ISession session)
        {
            _session = session;
        }

        public bool CanExecute => _session.CanIncreaseSpeed;
        
        public void Execute()
        {
            if (CanExecute == false)
                throw new InvalidOperationException();
            
            _session.IncreaseSpeed();
        }
    }
}