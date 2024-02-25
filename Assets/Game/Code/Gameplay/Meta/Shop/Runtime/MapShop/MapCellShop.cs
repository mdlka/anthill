using System;
using YellowSquad.Anthill.Meta.Wallet;

namespace YellowSquad.Anthill.Meta.Shop
{
    public class MapCellShop
    {
        private readonly IWallet _wallet;
        private int _currentCellPrice;

        public MapCellShop(IWallet wallet)
        {
            _wallet = wallet;
        }

        public bool CanBuyCell => _wallet.CanSpend(_currentCellPrice);

        public void Buy()
        {
            if (CanBuyCell == false)
                throw new InvalidOperationException();
            
            _wallet.Spend(_currentCellPrice);
            _currentCellPrice += 1;
        }

        public void Visualize(IMapShopView view)
        {
            view.Render(CanBuyCell, _currentCellPrice);
        }
    }
}
