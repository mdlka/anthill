using System;
using YellowSquad.Anthill.Meta.Wallet;

namespace YellowSquad.Anthill.Meta.Shop
{
    public abstract class BaseUpgrade : IUpgrade
    {
        private readonly IPriceList _priceList;
        private readonly IWallet _wallet;

        public BaseUpgrade(IPriceList priceList, IWallet wallet)
        {
            _priceList = priceList;
            _wallet = wallet;
        }

        public int CurrentPrice => _priceList.CurrentPrice;
        public bool CanPerform => _wallet.CanSpend(_priceList.CurrentPrice) && CanOnPerform;
        public bool IsMax => CurrentValue == MaxValue;

        public abstract int MaxValue { get; }
        public abstract int CurrentValue { get; }
        protected abstract bool CanOnPerform { get; }
        
        public void Perform()
        {
            if (CanPerform == false)
                throw new InvalidOperationException();
            
            _wallet.Spend(_priceList.CurrentPrice);
            _priceList.Next();
            
            OnPerform();
        }

        protected abstract void OnPerform();
    }
}