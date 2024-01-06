using System;
using System.Collections.Generic;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public class DefaultMovement : IMovement
    {
        private readonly MovementPath _path;
        private readonly MovementSettings _settings;

        private int _currentPathIndex;
        private int _currentStep;
        private float _elapsedTime;
        private IReadOnlyList<FracAxialCoordinate> _currentPath;
        private FracAxialCoordinate _randomOffset;

        public DefaultMovement(MovementPath path, MovementSettings settings, AxialCoordinate startPosition = default)
        {
            _path = path;
            _settings = settings;
            _currentPath = new FracAxialCoordinate[] { startPosition };
        }

        public bool ReachedTargetPosition => _currentPathIndex == 0;
        public FracAxialCoordinate CurrentPosition => 
            AxialCoordinateMath.Lerp(
                _currentPath[_currentPathIndex], 
                _currentPath[Math.Clamp(_currentPathIndex - 1, 0, _currentPathIndex)], 
                (float)_currentStep / _settings.StepsToGoal) 
            + _randomOffset;

        public void MoveTo(AxialCoordinate targetPosition)
        {
            if (ReachedTargetPosition == false)
                throw new InvalidOperationException();

            _currentPath = _path.Calculate(CurrentPosition, targetPosition);
            _currentPathIndex = _currentPath.Count - 1;
            _randomOffset = _settings.RandomOffset();
        }
        
        public void Update(float deltaTime)
        {
            if (ReachedTargetPosition)
                return;

            _elapsedTime += deltaTime;

            if (_elapsedTime < _settings.MoveToGoalDuration / _settings.StepsToGoal)
                return;

            _elapsedTime = 0;
            _currentStep += 1;

            if (_currentStep != _settings.StepsToGoal)
                return;

            _currentStep = 0;
            _currentPathIndex -= 1;
            
            if (ReachedTargetPosition)
                _randomOffset = new FracAxialCoordinate(0, 0);
        }
    }
}