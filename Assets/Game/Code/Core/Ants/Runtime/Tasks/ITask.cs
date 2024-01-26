using System;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public interface ITask : IEquatable<ITask>
    {
        AxialCoordinate TargetCellPosition { get; }
        bool Completed { get; }
        
        void Complete(FracAxialCoordinate currentPosition = default);
    }
}