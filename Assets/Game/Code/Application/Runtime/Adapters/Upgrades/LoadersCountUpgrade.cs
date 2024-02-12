using System;
using YellowSquad.Anthill.Meta;
using YellowSquad.Anthill.Session;

namespace YellowSquad.Anthill.Application
{
    internal class LoadersCountUpgrade : IUpgrade
    {
        private readonly ISession _session;

        public LoadersCountUpgrade(ISession session)
        {
            _session = session;
        }

        public int MaxValue => _session.MaxLoaders;
        public int CurrentValue => _session.CurrentLoaders;
        public bool CanPerform => _session.CanAddLoader;
        public int CurrentLevel { get; private set; }

        public void Perform()
        {
            if (CanPerform == false)
                throw new InvalidOperationException();
            
            _session.AddLoader();
            CurrentLevel += 1;
        }
    }
}