﻿using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public interface IMovement : IGameLoop
    {
        FracAxialCoordinate CurrentPosition { get; }
        bool ReachedTargetPosition { get; }

        void MoveTo(AxialCoordinate targetPosition);
    }
}