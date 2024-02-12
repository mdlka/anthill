using System;
using YellowSquad.Anthill.Meta;
using YellowSquad.Anthill.Session;

namespace YellowSquad.Anthill.Application
{
    internal class DiggersCountUpgrade : BaseUpgrade
    {
        private readonly ISession _session;

        public DiggersCountUpgrade(ISession session, IPriceList priceList, IWallet wallet) : base(priceList, wallet)
        {
            _session = session;
        }

        public override int MaxValue => _session.MaxDiggers;
        public override int CurrentValue => _session.CurrentDiggers;
        protected override bool CanOnPerform => _session.CanAddDigger;

        protected override void OnPerform()
        {
            if (CanOnPerform == false)
                throw new InvalidOperationException();
            
            _session.AddDigger();
        }
    }
}