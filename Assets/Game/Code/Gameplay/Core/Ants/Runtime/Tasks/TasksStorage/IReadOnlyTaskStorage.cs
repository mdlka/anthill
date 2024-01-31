namespace YellowSquad.Anthill.Core.Ants
{
    public interface IReadOnlyTaskStorage
    {
        bool HasFreeTaskGroup { get; }
        ITaskGroup FindTaskGroup();
    }
}