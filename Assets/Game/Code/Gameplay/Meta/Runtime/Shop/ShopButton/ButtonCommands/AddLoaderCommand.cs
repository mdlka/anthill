using System;
using YellowSquad.Anthill.Core.Ants;

namespace YellowSquad.Anthill.Meta
{
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
}