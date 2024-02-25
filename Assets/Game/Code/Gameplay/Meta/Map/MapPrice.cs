using YellowSquad.Anthill.Meta.Wallet;

namespace YellowSquad.Anthill.Meta.Map
{
    public class MapPrice
    {
        private readonly IWallet _wallet;
        private int _currentCellPrice;

        public MapPrice(IWallet wallet)
        {
            _wallet = wallet;
        }

        public bool CanBuyCell => _wallet.CanSpend(_currentCellPrice);

        public void Next()
        {
            _currentCellPrice += 1;
        }

        public void Visualize(IMapPriceView view)
        {
            view.Render(CanBuyCell, _currentCellPrice);
        }
    }
}
