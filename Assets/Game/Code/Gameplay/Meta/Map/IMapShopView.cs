namespace YellowSquad.Anthill.Meta.Map
{
    public interface IMapShopView
    {
        void Render(bool canBuyCell, int currentPrice);
    }
}