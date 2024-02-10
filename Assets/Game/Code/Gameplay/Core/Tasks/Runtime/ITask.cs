using System;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Tasks
{
    public interface ITask : IEquatable<ITask>
    {
        int Price { get; }
        FracAxialCoordinate TargetPosition { get; }
        TaskState State { get; }
        bool CanComplete { get; }

        void Execute();
        void Complete();
    }
}