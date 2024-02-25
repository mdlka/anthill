namespace YellowSquad.Anthill.Meta.Shop
{
    public interface IMapCellShopView
    {
        void Render(bool canBuyCell, int currentPrice);
    }
}