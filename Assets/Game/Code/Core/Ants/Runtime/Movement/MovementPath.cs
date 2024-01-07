using System;
using System.Collections.Generic;
using System.Linq;
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
                var neighborsCellsWithoutObstacle = _map.NeighborHexPositions(target)
                    .Where(position => _map.HasObstacleIn(position) == false).ToArray();

                if (neighborsCellsWithoutObstacle.Length == 0)
                    throw new InvalidOperationException("Invalid target position");

                currentTarget = neighborsCellsWithoutObstacle[Random.Range(0, neighborsCellsWithoutObstacle.Length)];
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

            return SmoothPath(rawTargetPath);
        }

        private IReadOnlyList<FracAxialCoordinate> SmoothPath(List<FracAxialCoordinate> rawTargetPath)
        {
            var targetPath = new List<FracAxialCoordinate>();
            var randomOffset = _settings.RandomOffset();

            for (int i = 0; i < rawTargetPath.Count - 1; i++)
            {
                var position = rawTargetPath[i];
                var nextPosition = rawTargetPath[i + 1];
                int targetStepsToGoal = _settings.StepsToGoal;

                if (i == 0 || i == rawTargetPath.Count - 2)
                {
                    float distance = HMath.Distance(position, nextPosition);

                    if (distance < 1)
                        targetStepsToGoal = (int)(targetStepsToGoal * distance);
                }

                for (int j = 0; j < targetStepsToGoal; j++)
                {
                    if (i == 0 && j == 0)
                        targetPath.Add(HMath.Lerp(position, nextPosition, (float)j / targetStepsToGoal));
                    
                    targetPath.Add(HMath.Lerp(position, nextPosition, (float)j / targetStepsToGoal) + randomOffset);
                }
            }

            targetPath.Add(rawTargetPath[^1]);

            return targetPath;
        }
    }
}