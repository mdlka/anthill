using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Tasks
{
    public interface ITaskGroup
    {
        AxialCoordinate TargetCellPosition { get; }
        bool AllTaskCompleted { get;  }
        bool HasFreeTask { get; }

        ITask ClosestTask(FracAxialCoordinate position);
    }
}