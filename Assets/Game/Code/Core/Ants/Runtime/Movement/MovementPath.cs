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

        public IReadOnlyList<FracAxialCoordinate> Calculate(FracAxialCoordinate start, AxialCoordinate target)
        {
            AxialCoordinate currentTarget = target;
            
            if (_map.HasObstacleIn(target))
            {
                IReadOnlyList<AxialCoordinate> neighborsCellsWithoutObstacle = _map.NeighborHexPositions(target, 
                        where: position => _map.HasObstacleIn(position) == false);

                if (neighborsCellsWithoutObstacle.Count == 0)
                    throw new InvalidOperationException("Invalid target position");

                currentTarget = neighborsCellsWithoutObstacle[Random.Range(0, neighborsCellsWithoutObstacle.Count)];
            }

            AxialCoordinate roundedStart = start.AxialRound();

            if (_path.Calculate(roundedStart, currentTarget, out var path) == false)
                throw new InvalidOperationException("Can't find path");

            var rawTargetPath = new List<FracAxialCoordinate>();

            if (currentTarget != target)
                rawTargetPath.Add(HMath.Lerp(currentTarget, target, 0.45f));

            foreach (var position in path)
                rawTargetPath.Add(position);

            if (roundedStart != start)
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
                nextPosition = i != rawTargetPath.Count - 1 ? (rawTargetPath[i] + rawTargetPath[i + 1]) * 0.5f : rawTargetPath[i];

                for (int j = 0; j < _settings.StepsToGoal; j++)
                {
                    var currentOffset = i != 0 ? randomOffset : HMath.Lerp(FracAxialCoordinate.Zero, randomOffset, (float)j / _settings.StepsToGoal);
                    targetPath.Add(HBezier.GetPoint(position, auxiliaryPosition, nextPosition, (float)j / _settings.StepsToGoal) + currentOffset);
                }
            }

            return targetPath;
        }
    }
}