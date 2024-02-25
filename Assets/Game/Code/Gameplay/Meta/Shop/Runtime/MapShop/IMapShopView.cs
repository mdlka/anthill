namespace YellowSquad.Anthill.Meta.Shop
{
    public interface IMapShopView
    {
        void Render(bool canBuyCell, int currentPrice);
    }
}