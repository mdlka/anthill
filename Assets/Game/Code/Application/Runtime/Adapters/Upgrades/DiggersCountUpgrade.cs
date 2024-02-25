using System;
using YellowSquad.Anthill.Core.Ants;
using YellowSquad.Anthill.Meta.Shop;
using YellowSquad.Anthill.Meta.Wallet;

namespace YellowSquad.Anthill.Application.Adapters
{
    internal class DiggersCountUpgrade : BaseUpgrade
    {
        private readonly IAnthill _anthill;

        public DiggersCountUpgrade(IAnthill anthill, IPriceList priceList, IWallet wallet) : base(priceList, wallet)
        {
            _anthill = anthill;
        }

        public override int MaxValue => _anthill.Diggers.MaxCount;
        public override int CurrentValue => _anthill.Diggers.CurrentCount;
        protected override bool CanOnPerform => _anthill.CanAddDigger;

        protected override void OnPerform()
        {
            if (CanOnPerform == false)
                throw new InvalidOperationException();
            
            _anthill.AddDigger();
        }
    }
}