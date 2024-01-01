namespace YellowSquad.Anthill.Core.Ants
{
    public interface IReadOnlyTaskStorage
    {
        bool HasTask { get; }
        ITask FindTask();
    }
}