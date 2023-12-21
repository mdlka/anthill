using System;
using System.Collections.Generic;
using YellowSquad.Anthill.Core.AStarPathfinding;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    internal class DefaultMovement : IMovement
    {
        private readonly float _moveDelay;
        private readonly IPath _path;

        private int _currentPathIndex;
        private float _elapsedTime;
        private IReadOnlyList<AxialCoordinate> _currentPath;
        private AxialCoordinate _targetPosition;

        public DefaultMovement(float moveDelay, IPath path)
        {
            _moveDelay = moveDelay;
            _path = path;
        }

        public AxialCoordinate CurrentPosition => _currentPath[_currentPathIndex];
        public bool ReachedTargetPosition => _targetPosition.Equals(CurrentPosition);
        
        public void MoveTo(AxialCoordinate targetPosition)
        {
            if (ReachedTargetPosition == false)
                throw new InvalidOperationException();

            if (_path.Calculate(CurrentPosition, targetPosition, out IReadOnlyList<AxialCoordinate> path) == false)
                throw new InvalidOperationException();

            _currentPathIndex = 0;
            _targetPosition = targetPosition;
            _currentPath = path;
        }
        
        public void Update(float deltaTime)
        {
            if (ReachedTargetPosition)
                return;

            _elapsedTime += deltaTime;

            if (_elapsedTime < _moveDelay)
                return;

            _elapsedTime = 0;
            _currentPathIndex += 1;
        }
    }
}