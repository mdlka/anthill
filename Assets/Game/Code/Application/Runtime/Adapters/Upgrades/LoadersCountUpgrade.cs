using System;
using YellowSquad.Anthill.Meta;
using YellowSquad.Anthill.Session;

namespace YellowSquad.Anthill.Application
{
    internal class LoadersCountUpgrade : BaseUpgrade
    {
        private readonly ISession _session;

        public LoadersCountUpgrade(ISession session, IPriceList priceList, IWallet wallet) : base(priceList, wallet)
        {
            _session = session;
        }

        public override int MaxValue => _session.MaxLoaders;
        public override int CurrentValue => _session.CurrentLoaders;
        protected override bool CanOnPerform => _session.CanAddLoader;

        protected override void OnPerform()
        {
            if (CanOnPerform == false)
                throw new InvalidOperationException();
            
            _session.AddLoader();
        }
    }
}