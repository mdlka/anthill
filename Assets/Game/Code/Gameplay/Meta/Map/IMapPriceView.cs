namespace YellowSquad.Anthill.Meta.Map
{
    public interface IMapPriceView
    {
        void Render(bool canBuyCell, int currentPrice);
    }
}