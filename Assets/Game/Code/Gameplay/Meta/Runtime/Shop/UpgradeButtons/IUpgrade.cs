namespace YellowSquad.Anthill.Meta
{
    public interface IUpgrade
    {
        int MaxValue { get; }
        int CurrentValue { get; }
        
        bool CanPerform { get; }
        void Perform();
    }
}