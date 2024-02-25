namespace YellowSquad.Anthill.Meta.Shop
{
    public interface IUpgrade
    {
        int MaxValue { get; }
        int CurrentValue { get; }
        int CurrentPrice { get; }
        
        bool IsMax { get; }
        
        bool CanPerform { get; }
        void Perform();
    }
}