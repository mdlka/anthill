using System;
using YellowSquad.Anthill.Meta.Wallet;

namespace YellowSquad.Anthill.Meta.Shop
{
    public class MapCellShop
    {
        private readonly IWallet _wallet;
        private readonly IPriceList _priceList;

        public MapCellShop(IWallet wallet, IPriceList priceList)
        {
            _wallet = wallet;
            _priceList = priceList;
        }

        public bool CanBuyCell => _wallet.CanSpend(_priceList.CurrentPrice);

        public void Buy()
        {
            if (CanBuyCell == false)
                throw new InvalidOperationException();
            
            _wallet.Spend(_priceList.CurrentPrice);
            
            if (_priceList.HasNext)
                _priceList.Next();
        }

        public void Visualize(IMapCellShopView view)
        {
            view.Render(CanBuyCell, _priceList.CurrentPrice);
        }
    }
}
