namespace YellowSquad.Anthill.Core.Ants
{
    public interface ITaskStorage : IReadOnlyTaskStorage
    {
        void AddTaskGroup(ITaskGroup taskGroup);
    }
}