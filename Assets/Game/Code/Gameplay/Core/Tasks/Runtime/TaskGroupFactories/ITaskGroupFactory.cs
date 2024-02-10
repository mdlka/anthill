using System;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Tasks
{
    public interface ITaskGroupFactory
    {
        bool CanCreate(AxialCoordinate targetPosition);
        ITaskGroup Create(AxialCoordinate targetPosition, Action onComplete = null);
    }
}