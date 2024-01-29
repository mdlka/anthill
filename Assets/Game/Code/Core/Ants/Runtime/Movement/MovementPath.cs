using System;
using System.Collections.Generic;
using UnityEngine;
using YellowSquad.Anthill.Core.AStarPathfinding;
using YellowSquad.Anthill.Core.HexMap;
using YellowSquad.HexMath;
using Random = UnityEngine.Random;

namespace YellowSquad.Anthill.Core.Ants
{
    public class MovementPath
    {
        private readonly IHexMap _map;
        private readonly IPath _path;
        private readonly MovementSettings _settings;
        private IReadOnlyList<FracAxialCoordinate> _lastCalculatedPath;

        public MovementPath(IHexMap map, IPath defaultPath, MovementSettings settings)
        {
            _map = map;
            _path = defaultPath;
            _settings = settings;
        }

        public IReadOnlyList<FracAxialCoordinate> Calculate(FracAxialCoordinate start, FracAxialCoordinate target, 
            Func<AxialCoordinate, FracAxialCoordinate> closestPosition = null)
        {
            AxialCoordinate roundedTarget = target.AxialRound();
            AxialCoordinate currentTarget = roundedTarget;
            
            if (_map.HasObstacleIn(roundedTarget))
            {
                IReadOnlyList<AxialCoordinate> neighborsCellsWithoutObstacle = _map.NeighborHexPositions(roundedTarget, 
                        where: position => _map.HasObstacleIn(position) == false);

                if (neighborsCellsWithoutObstacle.Count == 0)
                    throw new InvalidOperationException("Invalid target position");

                currentTarget = neighborsCellsWithoutObstacle[Random.Range(0, neighborsCellsWithoutObstacle.Count)];
            }

            AxialCoordinate roundedStart = start.AxialRound();

            if (_path.Calculate(roundedStart, currentTarget, out var path) == false)
                throw new InvalidOperationException("Can't find path");

            var rawTargetPath = new List<FracAxialCoordinate>();

            FracAxialCoordinate? calculatedClosestPosition = closestPosition?.Invoke(currentTarget);
            
            if (currentTarget != target && calculatedClosestPosition != null)
                rawTargetPath.Add(HMath.Lerp(currentTarget, calculatedClosestPosition.Value, 0.8f));

            foreach (var position in path)
                rawTargetPath.Add(position);

            if (HMath.Distance(roundedStart, start) > _settings.MaxRandomOffsetRadius)
                rawTargetPath.Add(start);

            return _lastCalculatedPath = SmoothPath(rawTargetPath);
        }

        public void OnDrawGizmos()
        {
            if (_lastCalculatedPath == null || _lastCalculatedPath.Count == 0)
                return;
            
            foreach (var position in _lastCalculatedPath)
                Gizmos.DrawSphere(position.ToVector3(_map.Scale), 0.2f);
        }

        private IReadOnlyList<FracAxialCoordinate> SmoothPath(IReadOnlyList<FracAxialCoordinate> rawTargetPath)
        {
            var randomOffset = _settings.RandomOffset();
            
            var targetPath = new List<FracAxialCoordinate>();
            var nextPosition = rawTargetPath[0];

            for (int i = 0; i < rawTargetPath.Count; i++)
            {
                var position = nextPosition;
                var auxiliaryPosition = rawTargetPath[i];
                nextPosition = rawTargetPath[i];

                if (i != rawTargetPath.Count - 1)
                    nextPosition = (rawTargetPath[i] + rawTargetPath[i + 1]) * 0.5f;

                if (i == 0 || i == rawTargetPath.Count - 1)
                    auxiliaryPosition = HMath.Lerp(position, nextPosition, 0.5f);

                int stepsToGoal = _settings.StepsToGoal;

                if (i == 0 || i == rawTargetPath.Count - 1)
                {
                    float distance = HMath.Distance(position, nextPosition);
                    
                    if (distance < 1)
                        stepsToGoal = Mathf.CeilToInt(stepsToGoal * 0.5f);
                }

                for (int j = 0; j < stepsToGoal; j++)
                {
                    var currentOffset = randomOffset;
                    
                    if (i == 0)
                        currentOffset = HMath.Lerp(FracAxialCoordinate.Zero, randomOffset, (float)j / stepsToGoal);
                    else if (i == rawTargetPath.Count - 1)
                        currentOffset = HMath.Lerp(randomOffset, FracAxialCoordinate.Zero, (float)j / stepsToGoal);
                    
                    targetPath.Add(HBezier.GetPoint(position, auxiliaryPosition, nextPosition, (float)j / stepsToGoal) + currentOffset);
                }
            }

            return targetPath;
        }
    }
}