namespace YellowSquad.Anthill.Meta
{
    public interface IWallet
    {
        int CurrentValue { get; }
        
        void Add(int value);
        void Spend(int value);
        bool CanSpend(int value);
    }
}
