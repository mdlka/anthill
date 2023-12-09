using System.Collections.Generic;
using YellowSquad.HexMath;

namespace YellowSquad.Anthill.Core.AStarPathfinding.Tests
{
    public class StubMovePolicy : IMovePolicy
    {
        private readonly HashSet<AxialCoordinate> _availablePositions;

        public StubMovePolicy(IEnumerable<AxialCoordinate> availablePositions)
        {
            _availablePositions = new HashSet<AxialCoordinate>(availablePositions);
        }

        public bool CanMove(AxialCoordinate axialCoordinate)
        {
            return _availablePositions.Contains(axialCoordinate);
        }
    }
}
