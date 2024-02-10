namespace YellowSquad.Anthill.Core.Tasks
{
    public interface IReadOnlyTaskStorage
    {
        bool HasFreeTaskGroup { get; }
        ITaskGroup FindTaskGroup();
    }
}