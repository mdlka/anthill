using System;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public interface ITaskGroup : IEquatable<ITaskGroup>
    {
        AxialCoordinate TargetCellPosition { get; }
        bool HasFreeTask { get; }

        ITask ClosestTask(FracAxialCoordinate position);
    }
}