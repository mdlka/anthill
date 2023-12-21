namespace YellowSquad.Anthill.Core.Ants
{
    public interface ITaskStorage : IReadOnlyTaskStorage
    {
        void AddTask(ITask task);
    }
}