namespace YellowSquad.Anthill.Meta
{
    public interface IPriceList
    {
        int CurrentPriceNumber { get; }
        int CurrentPrice{ get; }
        bool HasNext { get; }

        void Next();
    }
}