using System;
using YellowSquad.Anthill.Meta;

namespace YellowSquad.Anthill.Application
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