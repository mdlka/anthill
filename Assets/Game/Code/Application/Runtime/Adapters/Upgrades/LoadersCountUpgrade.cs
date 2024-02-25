using System;
using YellowSquad.Anthill.Core.Ants;
using YellowSquad.Anthill.Meta.Shop;
using YellowSquad.Anthill.Meta.Wallet;

namespace YellowSquad.Anthill.Application
{
    internal class LoadersCountUpgrade : BaseUpgrade
    {
        private readonly IAnthill _anthill;

        public LoadersCountUpgrade(IAnthill anthill, IPriceList priceList, IWallet wallet) : base(priceList, wallet)
        {
            _anthill = anthill;
        }

        public override int MaxValue => _anthill.Loaders.MaxCount;
        public override int CurrentValue => _anthill.Loaders.CurrentCount;
        protected override bool CanOnPerform => _anthill.CanAddLoader;

        protected override void OnPerform()
        {
            if (CanOnPerform == false)
                throw new InvalidOperationException();
            
            _anthill.AddLoader();
        }
    }
}