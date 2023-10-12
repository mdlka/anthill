using System.Collections.Generic;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.AStarPathfinding
{
    public class AvailableMovePolicy : IMovePolicy
    {
        private readonly HashSet<AxialCoordinate> _availablePositions;

        public AvailableMovePolicy(IEnumerable<AxialCoordinate> availablePositions)
        {
            _availablePositions = new HashSet<AxialCoordinate>(availablePositions);
        }

        public bool CanMove(AxialCoordinate axialCoordinate)
        {
            return _availablePositions.Contains(axialCoordinate);
        }
    }
}
