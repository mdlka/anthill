using System;
using YellowSquad.Anthill.Core.Ants;

namespace YellowSquad.Anthill.Meta
{
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
}