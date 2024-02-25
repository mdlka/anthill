using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Tasks
{
    public interface ITaskStorage : IReadOnlyTaskStorage
    {
        bool HasTaskGroupIn(AxialCoordinate position);
        void AddTaskGroup(ITaskGroup taskGroup);

        void Visualize(ITasksProgressView view);
    }
}