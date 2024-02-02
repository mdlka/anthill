using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public interface ITaskGroup
    {
        AxialCoordinate TargetCellPosition { get; }
        bool AllTaskCompleted { get;  }
        bool HasFreeTask { get; }

        ITask ClosestTask(FracAxialCoordinate position);
    }
}