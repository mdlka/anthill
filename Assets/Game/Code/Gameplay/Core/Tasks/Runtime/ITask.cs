using System;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Tasks
{
    public interface ITask : IEquatable<ITask>
    {
        int Price { get; }
        FracAxialCoordinate TargetPosition { get; }
        bool Completed { get; }
        bool Cancelled { get; }

        void UpdateProgress(float deltaTime);
        void Cancel();
    }
}