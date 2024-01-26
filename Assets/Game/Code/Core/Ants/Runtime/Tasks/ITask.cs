using System;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public interface ITask : IEquatable<ITask>
    {
        AxialCoordinate TargetCellPosition { get; }
        TaskState State { get; }
        bool CanComplete { get; }

        void Execute(FracAxialCoordinate position = default);
        void Complete();
    }
}