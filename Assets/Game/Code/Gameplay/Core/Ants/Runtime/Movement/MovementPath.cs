using System;
using System.Collections.Generic;
using System.Linq;
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
                currentTarget = NeighborPositionClosestToTarget(roundedTarget, start);

            AxialCoordinate roundedStart = start.AxialRound();
            AxialCoordinate currentStart = roundedStart;

            if (_map.HasObstacleIn(roundedStart))
                currentStart = NeighborPositionClosestToTarget(roundedStart, target);
            
            if (_path.Calculate(currentStart, currentTarget, out var path) == false)
                throw new InvalidOperationException("Can't find path");
            
            var rawTargetPath = new List<FracAxialCoordinate>();

            FracAxialCoordinate? calculatedClosestPosition = closestPosition?.Invoke(currentTarget);
            
            if (currentTarget != target && calculatedClosestPosition != null)
                rawTargetPath.Add(HMath.Lerp(currentTarget, calculatedClosestPosition.Value, 0.8f));
            
            if (currentTarget != roundedTarget && calculatedClosestPosition == null)
                rawTargetPath.Add(roundedTarget);

            foreach (var position in path)
                rawTargetPath.Add(position);
            
            if (currentStart != roundedStart)
                rawTargetPath.Add(roundedStart);

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
        
        private AxialCoordinate NeighborPositionClosestToTarget(AxialCoordinate position, FracAxialCoordinate target)
        {
            IReadOnlyList<AxialCoordinate> neighborsCellsWithoutObstacle = _map.NeighborHexPositions(position,
                where: pos => _map.HasObstacleIn(pos) == false);

            if (neighborsCellsWithoutObstacle.Count == 0)
                throw new InvalidOperationException(position.ToString());

            return neighborsCellsWithoutObstacle.Aggregate((pos1, pos2) =>
                HMath.Distance(pos1, target) > 
                HMath.Distance(pos2, target) ? pos2 : pos1);
        }

        private IReadOnlyList<FracAxialCoordinate> SmoothPath(IReadOnlyList<FracAxialCoordinate> rawTargetPath)
        {
            var randomOffset = _settings.RandomOffset();
            
            var targetPath = new List<FracAxialCoordinate>();
            var nextPosition = rawTargetPath[0];

            for (int i = 0; i < rawTargetPath.Count; i++)
            {
                var position = nextPosition;
                nextPosition = i == rawTargetPath.Count - 1 ? rawTargetPath[i] : HMath.Lerp(rawTargetPath[i], rawTargetPath[i + 1], 0.5f);
                
                var auxiliaryPosition = i != 0 && i != rawTargetPath.Count - 1 ? rawTargetPath[i] : HMath.Lerp(position, nextPosition, 0.5f);

                int stepsToNextPosition = _settings.StepsBetweenCells;

                if (i == 0 || i == rawTargetPath.Count - 1)
                {
                    float distance = HMath.Distance(position, nextPosition);
                    
                    if (distance < 1)
                        stepsToNextPosition = Mathf.CeilToInt(stepsToNextPosition * 0.5f);
                }

                for (int j = 0; j < stepsToNextPosition; j++)
                {
                    var currentOffset = randomOffset;
                    
                    if (i == 0)
                        currentOffset = HMath.Lerp(FracAxialCoordinate.Zero, randomOffset, (float)j / stepsToNextPosition);
                    else if (i == rawTargetPath.Count - 1)
                        currentOffset = HMath.Lerp(randomOffset, FracAxialCoordinate.Zero, (float)j / stepsToNextPosition);
                    
                    targetPath.Add(HBezier.GetPoint(position, auxiliaryPosition, nextPosition, (float)j / stepsToNextPosition) + currentOffset);
                }
            }

            return targetPath;
        }
    }
}