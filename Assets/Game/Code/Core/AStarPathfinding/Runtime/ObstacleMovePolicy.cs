using System.Collections.Generic;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.AStarPathfinding
{
    public class ObstacleMovePolicy : IMovePolicy
    {
        private readonly HashSet<AxialCoordinate> _obstacles;

        public ObstacleMovePolicy(IEnumerable<AxialCoordinate> obstacles)
        {
            _obstacles = new HashSet<AxialCoordinate>(obstacles);
        }

        public bool CanMove(AxialCoordinate axialCoordinate)
        {
            return _obstacles.Contains(axialCoordinate) == false;
        }
    }
}
