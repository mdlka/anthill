using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public interface ITaskStorage : IReadOnlyTaskStorage
    {
        bool HasTaskGroupIn(AxialCoordinate position);
        void AddTaskGroup(ITaskGroup taskGroup);
    }
}