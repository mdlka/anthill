using System;
using System.Collections.Generic;
using System.Linq;
using YellowSquad.Anthill.Core.AStarPathfinding;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public class DefaultMovement : IMovement
    {
        private readonly float _moveDelay;
        private readonly IPath _path;

        private int _currentPathIndex;
        private float _elapsedTime;
        private IReadOnlyList<AxialCoordinate> _currentPath;

        public DefaultMovement(float moveDelay, IPath path, AxialCoordinate startPosition = default)
        {
            _moveDelay = moveDelay;
            _path = path;
            _currentPath = new[] { startPosition };
        }

        public AxialCoordinate CurrentPosition => _currentPath[_currentPathIndex];
        public bool ReachedTargetPosition => _currentPathIndex == 0;
        
        public void MoveTo(AxialCoordinate targetPosition)
        {
            if (ReachedTargetPosition == false)
                throw new InvalidOperationException();

            if (_path.Calculate(CurrentPosition, targetPosition, out IReadOnlyList<AxialCoordinate> path) == false)
                throw new InvalidOperationException();

            _currentPathIndex = path.Count - 1;
            _currentPath = new List<AxialCoordinate>(path);
        }
        
        public void Update(float deltaTime)
        {
            if (ReachedTargetPosition)
                return;

            _elapsedTime += deltaTime;

            if (_elapsedTime < _moveDelay)
                return;

            _elapsedTime = 0;
            _currentPathIndex -= 1;
        }
    }
}