namespace YellowSquad.Anthill.Meta.Shop
{
    public interface IPriceList
    {
        int CurrentPriceNumber { get; }
        int CurrentPrice{ get; }
        bool HasNext { get; }

        void Next();
    }
}