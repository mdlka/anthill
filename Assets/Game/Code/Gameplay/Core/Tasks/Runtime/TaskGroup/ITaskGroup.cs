using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Tasks
{
    public interface ITaskGroup
    {
        AxialCoordinate TargetCellPosition { get; }
        bool AllTaskCompleted { get;  }
        bool HasFreeTask { get; }
        bool Cancelled { get; }
        
        float Progress { get; }

        ITask TakeClosestTask(FracAxialCoordinate position);
        void Cancel();
    }
}