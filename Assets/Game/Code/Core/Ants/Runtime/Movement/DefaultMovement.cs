using System;
using System.Collections.Generic;
using YellowSquad.Anthill.Core.AStarPathfinding;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.Ants
{
    public class DefaultMovement : IMovement
    {
        private readonly float _moveToGoalDuration;
        private readonly int _stepsToGoal;
        private readonly IPath _path;

        private int _currentPathIndex;
        private int _currentStep;
        private float _elapsedTime;
        private IReadOnlyList<AxialCoordinate> _currentPath;

        public DefaultMovement(float moveToGoalDuration, int stepsToGoal, IPath path, AxialCoordinate startPosition = default)
        {
            _moveToGoalDuration = moveToGoalDuration / stepsToGoal;
            _stepsToGoal = stepsToGoal;
            _path = path;
            _currentPath = new[] { startPosition };
        }

        public FracAxialCoordinate CurrentPosition => _currentPath[_currentPathIndex].Lerp(
            _currentPath[Math.Clamp(_currentPathIndex - 1, 0, _currentPathIndex)], (float)_currentStep / _stepsToGoal);
        public bool ReachedTargetPosition => _currentPathIndex == 0;
        
        public void MoveTo(AxialCoordinate targetPosition)
        {
            if (ReachedTargetPosition == false)
                throw new InvalidOperationException();

            if (_path.Calculate(CurrentPosition.AxialRound(), targetPosition, out IReadOnlyList<AxialCoordinate> path) == false)
                throw new InvalidOperationException();

            _currentPathIndex = path.Count - 1;
            _currentPath = new List<AxialCoordinate>(path);
        }
        
        public void Update(float deltaTime)
        {
            if (ReachedTargetPosition)
                return;

            _elapsedTime += deltaTime;

            if (_elapsedTime < _moveToGoalDuration)
                return;

            _elapsedTime = 0;
            _currentStep += 1;

            if (_currentStep != _stepsToGoal)
                return;

            _currentStep = 0;
            _currentPathIndex -= 1;
        }
    }
}