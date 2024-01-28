using System;
using System.Collections.Generic;
using System.Linq;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public class DefaultMovement : IMovement
    {
        private readonly MovementPath _path;
        private readonly MovementSettings _settings;

        private float _elapsedTime;
        private int _currentPathIndex;
        private IReadOnlyList<FracAxialCoordinate> _currentPath;
        private AxialCoordinate _lastStartPosition;
        private AxialCoordinate _lastTargetPosition;

        public DefaultMovement(MovementPath path, MovementSettings settings, AxialCoordinate startPosition = default)
        {
            _path = path;
            _settings = settings;
            _currentPath = new FracAxialCoordinate[] { startPosition };
        }

        public bool ReachedTargetPosition => _currentPathIndex == 0;
        public FracAxialCoordinate CurrentPosition => _currentPath[_currentPathIndex];

        public void MoveTo(FracAxialCoordinate targetPosition, Func<AxialCoordinate, FracAxialCoordinate> closestPosition = null)
        {
            if (ReachedTargetPosition == false)
                throw new InvalidOperationException();

            if (PossibleToReverse(targetPosition))
            {
                _currentPath = _currentPath.Reverse().ToList();
                _currentPathIndex = _currentPath.Count - 1;

                return;
            }

            _lastStartPosition = CurrentPosition.AxialRound();
            _lastTargetPosition = targetPosition.AxialRound();
            
            _currentPath = _path.Calculate(CurrentPosition, targetPosition, closestPosition);
            _currentPathIndex = _currentPath.Count - 1;
        }

        public void Update(float deltaTime)
        {
            if (ReachedTargetPosition)
                return;

            _elapsedTime += deltaTime;

            if (_elapsedTime < _settings.NormalizedMoveDuration)
                return;

            _elapsedTime = 0;
            _currentPathIndex -= 1;
        }
        
        private bool PossibleToReverse(FracAxialCoordinate targetPosition)
        {
            return _lastStartPosition != _lastTargetPosition 
                   && CurrentPosition.AxialRound() == _lastTargetPosition 
                   && targetPosition.AxialRound() == _lastStartPosition;
        }
    }
}