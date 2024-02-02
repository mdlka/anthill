namespace YellowSquad.Anthill.Meta
{
    internal interface IPriceList
    {
        int CurrentPriceNumber { get; }
        int CurrentPrice{ get; }
        bool HasNext { get; }

        void Next();
    }
}