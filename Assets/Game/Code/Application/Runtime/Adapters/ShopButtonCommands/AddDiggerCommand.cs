using System;
using YellowSquad.Anthill.Meta;
using YellowSquad.Anthill.Session;

namespace YellowSquad.Anthill.Application
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