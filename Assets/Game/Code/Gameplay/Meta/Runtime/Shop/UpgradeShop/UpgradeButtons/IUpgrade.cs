namespace YellowSquad.Anthill.Meta
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