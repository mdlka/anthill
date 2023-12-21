﻿using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    internal interface IMovement : IGameLoop
    {
        AxialCoordinate CurrentPosition { get; }
        bool ReachedTargetPosition { get; }

        void MoveTo(AxialCoordinate targetPosition);
    }
}