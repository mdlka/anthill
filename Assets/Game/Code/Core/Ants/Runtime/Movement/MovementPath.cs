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

        public MovementPath(IHexMap map, IPath defaultPath)
        {
            _map = map;
            _path = defaultPath;
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

            var targetPath = new List<FracAxialCoordinate>();

            if (currentTarget != target)
                targetPath.Add(HMath.Lerp(currentTarget, target, 0.5f));

            foreach (var position in path)
                targetPath.Add(position);

            if (roundedStart != start)
                targetPath.Add(start);

            return targetPath;
        }
    }
}