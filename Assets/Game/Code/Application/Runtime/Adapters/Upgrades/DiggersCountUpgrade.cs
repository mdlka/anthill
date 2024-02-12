using System;
using YellowSquad.Anthill.Meta;
using YellowSquad.Anthill.Session;

namespace YellowSquad.Anthill.Application
{
    internal class DiggersCountUpgrade : IUpgrade
    {
        private readonly ISession _session;

        public DiggersCountUpgrade(ISession session)
        {
            _session = session;
        }

        public int MaxValue => _session.MaxDiggers;
        public int CurrentValue => _session.CurrentDiggers;
        public bool CanPerform => _session.CanAddDigger;

        public void Perform()
        {
            if (CanPerform == false)
                throw new InvalidOperationException();
            
            _session.AddDigger();
        }
    }
}