using System;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public interface ITask : IEquatable<ITask>
    {
        FracAxialCoordinate TargetPosition { get; }
        TaskState State { get; }
        bool CanComplete { get; }

        void Execute();
        void Complete();
    }
}